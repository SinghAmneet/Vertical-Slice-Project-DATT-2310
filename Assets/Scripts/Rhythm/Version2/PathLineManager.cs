using UnityEngine;

public class PathLineManager : MonoBehaviour
{
    [Header("References")]
    public LineRenderer line;
    public SongController songController;

    [Header("Curve")]
    [Range(8, 80)] public int curveSegments = 24;   // more segments = smoother curve
    public float curveBend = 1.2f;                  // how “curvy” it is
    public bool alternateCurveSide = true;          // flips curve side each note for variety

    [Header("Visuals")]
    public float baseWidth = 0.05f;
    public float pulseWidthAdd = 0.03f;
    public float pulseAlphaAdd = 0.25f;
    public float pulseDecaySpeed = 8f;

    [Header("Animation")]
    [Tooltip("How fast the curve grows toward the next note (0..1). Higher = faster.")]
    public float growSpeed = 4f;

    [Header("Fade Rules")]
    [Tooltip("How much to fade once the next note becomes active (0 = none, 1 = full).")]
    [Range(0f, 1f)] public float fadeWhenActive = 0.85f;

    private Transform from;
    private Transform to;
    private NoteObject nextNote;

    private float growT = 0f;        // 0..1
    private float pulseT = 0f;       // 0..1 (decays)
    private double lastBeatIndex = -999;

    private int curveFlip = 1;       // +1 / -1 (used when alternateCurveSide = true)

    void Awake()
    {
        if (line == null) line = GetComponent<LineRenderer>();

        if (line != null)
        {
            line.enabled = false;
            line.positionCount = curveSegments;
            line.useWorldSpace = true;
        }
    }

    void Update()
    {
        if (line == null || songController == null)
            return;

        if (from == null || to == null)
        {
            line.enabled = false;
            return;
        }

        line.enabled = true;

        // -----------------------------
        // Beat pulse (based on BPM)
        // -----------------------------
        double songTime = songController.GetSongTime();
        if (songTime >= 0)
        {
            double spb = songController.GetSecondsPerBeat();
            double beatIndex = System.Math.Floor(songTime / spb);

            if (beatIndex != lastBeatIndex)
            {
                lastBeatIndex = beatIndex;
                pulseT = 1f;
            }
        }

        pulseT = Mathf.MoveTowards(pulseT, 0f, Time.deltaTime * pulseDecaySpeed);

        // -----------------------------
        // Grow animation
        // -----------------------------
        growT = Mathf.MoveTowards(growT, 1f, Time.deltaTime * growSpeed);

        // -----------------------------
        // Fade when next note becomes active
        // -----------------------------
        float activeFade = 0f;
        if (nextNote != null && nextNote.IsActiveState())
            activeFade = fadeWhenActive;

        // Width + alpha styling
        float width = baseWidth + (pulseWidthAdd * pulseT);
        line.startWidth = width;
        line.endWidth = width;

        float alpha = Mathf.Clamp01(0.6f + (pulseAlphaAdd * pulseT) - activeFade);

        // Apply a simple alpha gradient (slightly fades toward the end)
        Gradient g = new Gradient();
        Color c0 = line.startColor; c0.a = alpha;
        Color c1 = line.endColor;   c1.a = alpha * 0.6f;

        g.SetKeys(
            new GradientColorKey[] {
                new GradientColorKey(c0, 0f),
                new GradientColorKey(c1, 1f)
            },
            new GradientAlphaKey[] {
                new GradientAlphaKey(c0.a, 0f),
                new GradientAlphaKey(c1.a, 1f)
            }
        );
        line.colorGradient = g;

        // -----------------------------
        // Draw the CURVED line
        // -----------------------------
        Vector3 A = from.position;
        Vector3 C = to.position;

        // Control point B: offset perpendicular to the A->C direction
        Vector3 mid = (A + C) * 0.5f;
        Vector3 dir = (C - A);
        Vector3 perp = new Vector3(-dir.y, dir.x, 0f).normalized;

        // Bend depends on distance so it scales nicely at any spacing
        float dist = dir.magnitude;
        float bendAmount = curveBend * Mathf.Clamp(dist * 0.25f, 0.5f, 3.0f);

        Vector3 B = mid + perp * bendAmount * curveFlip;

        // Ensure the renderer has enough points
        if (line.positionCount != curveSegments)
            line.positionCount = curveSegments;

        // We want the curve to “grow” from 0..growT (like drawing it out)
        for (int i = 0; i < curveSegments; i++)
        {
            float t = (curveSegments == 1) ? 1f : (float)i / (curveSegments - 1);

            // Clamp drawing to the current grow amount
            float drawT = Mathf.Min(t, growT);

            Vector3 p = QuadraticBezier(A, B, C, drawT);
            line.SetPosition(i, p);
        }
    }

    // Called whenever a new note spawns: last note -> new note
    public void SetCurrentAndNext(NoteObject current, NoteObject next)
    {
        if (current == null || next == null) return;

        from = current.hitCircle != null ? current.hitCircle : current.transform;
        to   = next.hitCircle != null ? next.hitCircle : next.transform;

        nextNote = next;

        // Reset the draw animation each time we switch targets
        growT = 0f;

        // Optionally flip curve side each spawn (looks more organic)
        if (alternateCurveSide)
            curveFlip *= -1;
    }

    public void Clear()
    {
        from = null;
        to = null;
        nextNote = null;
        growT = 0f;
        pulseT = 0f;

        if (line != null) line.enabled = false;
    }

    // Quadratic Bezier: A -> B -> C
    static Vector3 QuadraticBezier(Vector3 A, Vector3 B, Vector3 C, float t)
    {
        float u = 1f - t;
        return (u * u) * A + (2f * u * t) * B + (t * t) * C;
    }
}
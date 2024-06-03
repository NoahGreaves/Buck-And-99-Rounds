using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Renderer))]
public class FlickeringEmissive : MonoBehaviour
{
    [SerializeField] private bool _flicker = true;

    [SerializeField, Min(0)] private float _flickerSpeed = 1f;
    [SerializeField] private AnimationCurve _brightnessCurve;

    private Renderer _renderer;
    private List<Material> _materials = new();
    private List<Color> _initColours = new();

    private const string EMISSIVE_COLOUR_NAME = "_EmissionColor";
    private const string EMISSIVE_KEYWORD = "_EMISSION";

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _brightnessCurve.postWrapMode = WrapMode.Loop;
        print(_renderer.materials.Length);
        foreach (Material material in _renderer.materials)
        {
            if (_renderer.material.enabledKeywords.Any(item => item.name == EMISSIVE_KEYWORD)
                && _renderer.material.HasColor(EMISSIVE_COLOUR_NAME))
            {
                _materials.Add(material);
                _initColours.Add(material.GetColor(EMISSIVE_COLOUR_NAME));
                continue;
            }

            Debug.LogError($"{material.name} is not configured to me emissive. \n FlickingerEmissive on {name} cannot animate material!");
        }

        if (_materials.Count == 0)
            this.enabled = false;
    }

    private void Update()
    {
        // if Flicker bool is FALSE, or the renderer is NOT VISIBLE --> Return
        if (!_flicker || !_renderer.isVisible)
            return;

        float scaledTime = Time.time * _flickerSpeed;
        for (int i = 0; i < _materials.Count; i++)
        {
            Color color = _initColours[i];

            float brightness = _brightnessCurve.Evaluate(scaledTime);
            color = new Color(
                color.r * Mathf.Pow(2, brightness),
                color.g * Mathf.Pow(2, brightness),
                color.b * Mathf.Pow(2, brightness),
                color.a
            );

            _materials[i].SetColor(EMISSIVE_COLOUR_NAME, color);
        }
    }
}

using UnityEngine;

public class PlayerRunForwardEventsHandler : RunForwardEventsHandler
{
    [SerializeField] private ParticleSystem _leftLegStepParticleSystem;
    [SerializeField] private ParticleSystem _rightLegStepParticleSystem;

    public override void LeftLeg()
    {
        _leftLegStepParticleSystem.Play();
    }

    public override void RightLeg()
    {
        _rightLegStepParticleSystem.Play();
    }
}
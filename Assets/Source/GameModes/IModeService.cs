using System;
using Services;

namespace Gamemodes
{
    public interface IModeService : IService
    {
        event Action<bool> OnComplete;
        
        void Begin();
    }
}
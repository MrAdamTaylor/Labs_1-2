using Infrastructure.StateMachine;
using UnityEngine;

namespace Infrastructure.Bootstrap
{
    public class GameBootstraper : MonoBehaviour
    {
        [SerializeField] private BootstrapConfigs _bootstrapConfigs;
        
        private Game _game;

        private void Awake()
        {
            _game = new Game(_bootstrapConfigs);
            _game.StateMachine.Enter<BootstrapState>();
        }
    }
}
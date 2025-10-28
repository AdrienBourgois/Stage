// Ceci est un commentaire
using UnityEngine; // Utiliser le code de Unity

namespace Stage
{
    [RequireComponent(typeof(CharacterController))]
    public class Player : MonoBehaviour
    {
        CharacterController controller;
        public float Speed = 5f;
        public float JumpHeight = 1.5f;

        Vector3 velocity;

        void Start()
        {
            controller = GetComponent<CharacterController>();
        }

        void Update()
        {
            Vector3 move = Vector3.zero;

            // Déplacements avant/arrière = axe Z
            if (Input.GetKey(KeyCode.Z))
                move.z += 1f;
            if (Input.GetKey(KeyCode.S))
                move.z -= 1f;

            // Déplacements gauche/droite = axe X
            if (Input.GetKey(KeyCode.D))
                move.x += 1f;
            if (Input.GetKey(KeyCode.Q))
                move.x -= 1f;

            // Normalisation et application de la vitesse
            move = move.normalized * Speed;

            // Gravité + saut
            if (controller.isGrounded)
            {
                if (velocity.y < 0)
                    velocity.y = -2f; // garde le joueur au sol

                if (Input.GetKeyDown(KeyCode.Space))
                    velocity.y = Mathf.Sqrt(JumpHeight * -2f * Physics.gravity.y);
            }

            velocity.y += Physics.gravity.y * Time.deltaTime;

            controller.Move((move + velocity) * Time.deltaTime);
        }
    }
}
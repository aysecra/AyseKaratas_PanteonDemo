using PanteonDemo.Interfaces;
using UnityEngine;

namespace PanteonDemo.Controller
{
    public static class MovementController
    {
        public static void MoveWithPath(IMovableWithPath client, Vector3[] path)
        {
           client.GoPath(path); 
        }
    }
}

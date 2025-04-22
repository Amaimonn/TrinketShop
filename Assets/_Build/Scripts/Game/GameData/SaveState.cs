// using System;
// using UnityEngine;

// namespace TrinketShop.Game.GameData
// {
//     [Serializable]
//     public abstract class SaveState<T> : IVersioned, ICopyable<T> where T : SaveState<T>
//     {
//         public int Version { get => _version; set => _version = value; }
//         [SerializeField] protected int _version = 1;

//         public abstract T Copy();
//     }
// }
﻿#nullable enable
using System.Collections.Generic;
using DarkFrontier.Attributes;
using DarkFrontier.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DarkFrontier.Positioning.Navigation
{
    public class NavigationPathfinder : MonoBehaviour
    {
        [field: SerializeReference, ReadOnly]
        public NavigationRegistry Registry { get; private set; } = null!;

        public bool standalone;

        private void Start()
        {
            if (standalone) Initialize(gameObject.scene);
        }

        public void Initialize(Scene scene)
        {
            Registry = gameObject.AddOrGet<NavigationRegistry>();
            
            Registry.Colliders.Clear();

            var colliders = FindObjectsOfType<NavigationCollider>();
            for(int i = 0, l = colliders.Length; i < l; i++)
            {
                if(colliders[i].gameObject.scene == scene)
                {
                    Registry.Add(colliders[i]);
                }
            }
        }

        private void Update()
        {
            if (standalone) Tick();
        }

        public void Tick()
        {
            for(int i = 0, l = Registry.Colliders.Count; i < l; i++)
            {
                Registry.Colliders[i].Regenerate();
            }
        }

        public List<Vector3> GetRoute(Vector3 start, Vector3 end, int depth = 0)
        {
            // Ensure destination is not contained by any Aabb
            end = MovePointOutOfAabb(end);
            if(depth > 1)
            {
                return new List<Vector3> { end };
            }
            // ReSharper disable once TooWideLocalVariableScope
            IntersectRay ray;
            var current = start;
            var ret = new List<Vector3>();
            for(var i = 0; i < 10; i++)
            {
                var intersected = GetClosestIntersectingAabb(ray = IntersectRay.FromEndpoints(current, end));
                if(intersected == null)
                {
                    ret.Add(end);
                    break;
                }
                var cross = Vector3.Cross(ray.dir, Vector3.up);
                var offset = cross * intersected.RoughSize();
                ret.AddRange(GetRoute(current, current = MovePointOutOfAabb((intersected.min + intersected.max) / 2 + offset), depth + 1));
            }
            return ret;
        }

        private Vector3 MovePointOutOfAabb(Vector3 point)
        {
            // ReSharper disable once TooWideLocalVariableScope
            Aabb? box;
            for(int i = 0, l = Registry.Colliders.Count; i < l; i++)
            {
                box = Registry.Colliders[i].Aabb;
                if(!box.Contains(point)) continue;
                var dir = (point - (box.min + box.max) / 2).normalized;
                if(dir == Vector3.zero) dir = Vector3.up;
                return MovePointOutOfAabb(point + dir * box.RoughSize());
            }

            return point;
        }

        private Aabb? GetClosestIntersectingAabb(IntersectRay ray)
        {
            // ReSharper disable once TooWideLocalVariableScope
            // ReSharper disable once InlineOutVariableDeclaration
            float dist, minDist = float.MaxValue;
            // ReSharper disable once TooWideLocalVariableScope
            Aabb? box, minBox = null;
            for(int i = 0, l = Registry.Colliders.Count; i < l; i++)
            {
                box = Registry.Colliders[i].Aabb;
                if(!GeometryUtils.Intersects(box, ray, out dist)) continue;
                if(dist >= minDist) continue;
                minDist = dist;
                minBox = box;
            }
            return minBox;
        }
    }
}

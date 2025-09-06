using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using ZLinq;

namespace Quality.Core.Utilities
{
    public static class RandomUtility
    {
        // Random 1 giá trị kiểu enum
        private static readonly System.Random s_random = new ();
        
        // Random thứ tự 1 list
        public static List<T> RandomList<T>(List<T> list, int amount)
        {
            return list.AsValueEnumerable().OrderBy(_ => Guid.NewGuid()).Take(amount).ToList();
        }

        // Lấy kết quả theo tỷ lệ xác suất
        public static bool Chance(float rand, float max = 100f)
        {
            return UnityEngine.Random.Range(0, max) < rand;
        }
        
        // Random 1 giá trị kiểu enum
        public static T RandomEnumValue<T>()
        {
            var v = Enum.GetValues(typeof(T));
            return (T)v.GetValue(s_random.Next(v.Length));
        }

        // Random 1 vị trí navmesh
        public static Vector3 GetRandomPosOnNavMesh(Vector3 center, float maxDistance)
        {
            // Lấy một điểm ngẫu nhiên bên trong hình cầu mà vị trí là tâm và bán kính là maxDistance.
            Vector3 randomPos = UnityEngine.Random.insideUnitSphere * maxDistance + center;

            // Từ vị trí ngẫu nhiên (randomPos), tìm điểm gần nhất trên bề mặt NavMesh trong phạm vi maxDistance.
            NavMesh.SamplePosition(randomPos, out var hit, maxDistance, NavMesh.AllAreas);

            return hit.position;
        }
    }
}

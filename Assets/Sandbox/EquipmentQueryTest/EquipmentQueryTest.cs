using System;
using System.Collections.Generic;
using System.Diagnostics;
using DarkFrontier.Foundation.Behaviors;
using Debug = UnityEngine.Debug;

namespace DarkFrontier.Sandbox.EquipmentQueryTest {
    public class EquipmentQueryTest : ComponentBehavior {
        public int uCount = 100;
        public int uRepetitions = 100;

        private List<int> iInts = new List<int>();
        private int iLength;
        
        public override void Initialize() {
            for (var lRepetition = 0; lRepetition < uRepetitions; lRepetition++) {
                for (var lCount = 0; lCount < uCount; lCount++) {
                    iInts.Add(lCount);
                }
            }
            iLength = uCount * uRepetitions;
            
            GC.Collect();
            
            var lStopwatch = new Stopwatch();
            lStopwatch.Stop();
            lStopwatch.Reset();
            
            // Arrays
            
            lStopwatch.Start();

            var lArrayRet = ArrayBased(uCount - 1);
            var lArrayLength = lArrayRet.Length;
            var lArraySum = 0;
            for (var lIndex = 0; lIndex < lArrayLength; lIndex++) {
                lArraySum += lArrayRet[lIndex];
            }
            
            lStopwatch.Stop();

            Debug.Log($"Array: {lStopwatch.ElapsedMilliseconds.ToString()} ({lArraySum})");
            
            lStopwatch.Reset();
            
            // Lists
            
            lStopwatch.Start();

            var lListRet = ListBased(uCount - 1);
            var lListLength = lListRet.Count;
            var lListSum = 0;
            for (var lIndex = 0; lIndex < lListLength; lIndex++) {
                lListSum += lListRet[lIndex];
            }
            
            lStopwatch.Stop();
            
            Debug.Log($"List: {lStopwatch.ElapsedMilliseconds.ToString()} ({lListSum})");
        }

        private int[] ArrayBased(int aQuery) {
            var lOccurrences = 0;
            for (var lIndex = 0; lIndex < iLength; lIndex++) {
                if (iInts[lIndex] == aQuery) {
                    lOccurrences++;
                }
            }
            var lRet = new int[lOccurrences];
            var lCounter = 0;
            for (var lIndex = 0; lIndex < iLength; lIndex++) {
                if (iInts[lIndex] != aQuery) continue;
                lRet[lCounter] = lIndex;
                lCounter++;
            }
            return lRet;
        }

        private List<int> ListBased(int aQuery) {
            var lRet = new List<int>(iLength);
            for (var lIndex = 0; lIndex < iLength; lIndex++) {
                if (iInts[lIndex] == aQuery) {
                    lRet.Add(lIndex);
                }
            }
            return lRet;
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;
using UnityEngine;

namespace ThirtyFifteenCurb {
    public static class CurbPatch {

        public static IEnumerable<CodeInstruction> Transpiler(ILGenerator il, IEnumerable<CodeInstruction> instructions) {
            Debug.Log("[30-15Curb] Patching the code...");
            List<CodeInstruction> codeInstructions = instructions.ToList();
            int index = codeInstructions.FindIndex(intr => intr.opcode.Equals(OpCodes.Ldc_I4_2));
            if (index != -1 && codeInstructions[index + 3].opcode.Equals(OpCodes.Call)) {
                Debug.Log("[30-15Curb] Inserting UpdateCurb instruction");
                codeInstructions.Insert(index + 3, new CodeInstruction(OpCodes.Call, typeof(CurbPatch).GetMethod(nameof(UpdateCurbHeight))));
                Debug.Log("[30-15Curb] Patching done!");
            } else {
                Debug.Log("[30-15Curb] Instruction seem to be wrong! Patching aborted!");
            }

            return codeInstructions;
        }

        public static void UpdateCurbHeight() {
            Debug.Log("[30-15Curb] Applying Curb height modification...");
            int netInfosCount = PrefabCollection<NetInfo>.LoadedCount();
            Debug.Log($"[30-15Curb] Found {netInfosCount} NetInfo's");
            for (uint i = 0; i < netInfosCount; i++) {
                var network = PrefabCollection<NetInfo>.GetPrefab(i);
                if (network == null) continue;
                if (network.m_netAI == null) continue;
                if (network.m_netAI.GetType().Name != "RoadAI" &&
                    network.m_netAI.GetType().Name != "RoadBridgeAI" &&
                    network.m_netAI.GetType().Name != "RoadTunnelAI") continue;
                {
                    var affect = false;
                    if (network.m_segments.Length != 0) {
                        for (uint j = 0; j < network.m_segments.Length; j++) {
                            if (network.m_segments[j].m_segmentMesh == null) continue;
                            if (network.m_segments[j].m_segmentMaterial == null) continue;
                            if (network.m_segments[j].m_segmentMaterial.shader.name != "Custom/Net/Road" &&
                                network.m_segments[j].m_segmentMaterial.shader.name != "Custom/Net/RoadBridge" &&
                                network.m_segments[j].m_segmentMaterial.shader.name != "Custom/Net/TrainBridge") continue;
                            var vertices = network.m_segments[j].m_segmentMesh.vertices;
                            var fifteen = 0;
                            var thirty = 0;
                            for (int k = 0; k < vertices.Length; k++) {
                                if (vertices[k].y < -0.24f && vertices[k].y > -0.31f) thirty++;
                                if (vertices[k].y < -0.145f && vertices[k].y > -0.155f) fifteen++;
                            }

                            if (fifteen < 8 && thirty > 8) affect = true;
                        }
                    }

                    if (network.m_lanes == null) continue;
                    if (network.m_lanes.Length != 0 && affect == true) {
                        for (uint j = 0; j < network.m_lanes.Length; j++) {
                            if (network.m_lanes[j].m_verticalOffset == -0.3f) network.m_lanes[j].m_verticalOffset = -0.15f;
                        }
                    }

                    if (network.m_surfaceLevel == -0.3f && affect == true) network.m_surfaceLevel = -0.15f;
                }
            }

            for (uint i = 0; i < netInfosCount; i++) {
                var network = PrefabCollection<NetInfo>.GetPrefab(i);
                if (network == null) continue;
                if (network.m_netAI == null) continue;
                if (network.m_netAI.GetType().Name != "RoadAI" &&
                    network.m_netAI.GetType().Name != "RoadBridgeAI" &&
                    network.m_netAI.GetType().Name != "RoadTunnelAI") continue;
                {
                    var affect = false;
                    if (network.m_segments.Length != 0) {
                        for (uint j = 0; j < network.m_segments.Length; j++) {
                            if (network.m_segments[j].m_segmentMesh == null) continue;
                            if (network.m_segments[j].m_segmentMaterial == null) continue;
                            if (network.m_segments[j].m_segmentMaterial.shader.name != "Custom/Net/Road" &&
                                network.m_segments[j].m_segmentMaterial.shader.name != "Custom/Net/RoadBridge" &&
                                network.m_segments[j].m_segmentMaterial.shader.name != "Custom/Net/TrainBridge") continue;
                            var vertices = network.m_segments[j].m_segmentMesh.vertices;
                            var fifteen = 0;
                            var thirty = 0;
                            for (int k = 0; k < vertices.Length; k++) {
                                if (vertices[k].y < -0.24f && vertices[k].y > -0.31f) thirty++;
                                if (vertices[k].y < -0.145f && vertices[k].y > -0.155f) fifteen++;
                            }

                            if (fifteen < 8 && thirty > 8) {
                                affect = true;
                                var vertexArray = new Vector3[vertices.Length];
                                vertices.CopyTo(vertexArray, 0);
                                for (int l = 0; l < vertices.Length; l++) {
                                    if (vertexArray[l].y < 0.0f && vertexArray[l].y > -0.31f) vertexArray[l].y = vertexArray[l].y / 2.0f;
                                }

                                network.m_segments[j].m_segmentMesh.vertices = vertexArray;
                            }
                        }
                    }

                    if (network.m_nodes.Length != 0) {
                        for (uint j = 0; j < network.m_nodes.Length; j++) {
                            if (network.m_nodes[j].m_nodeMesh == null) continue;
                            if (network.m_nodes[j].m_nodeMaterial == null) continue;
                            if (network.m_nodes[j].m_nodeMaterial.shader.name != "Custom/Net/Road" &&
                                network.m_nodes[j].m_nodeMaterial.shader.name != "Custom/Net/RoadBridge" &&
                                network.m_nodes[j].m_nodeMaterial.shader.name != "Custom/Net/TrainBridge") continue;
                            var vertices = network.m_nodes[j].m_nodeMesh.vertices;
                            var fifteen = 0;
                            var thirty = 0;
                            for (int k = 0; k < vertices.Length; k++) {
                                if (vertices[k].y < -0.16f && vertices[k].y > -0.31f) thirty++;
                                if (vertices[k].y < -0.148f && vertices[k].y > -0.152f) fifteen++;
                            }

                            if (fifteen < 20 && thirty > 4) {
                                var vertexArray = new Vector3[vertices.Length];
                                vertices.CopyTo(vertexArray, 0);
                                for (int l = 0; l < vertices.Length; l++) {
                                    if (vertexArray[l].y < 0.0f && vertexArray[l].y > -0.31f) vertexArray[l].y = vertexArray[l].y / 2.0f;
                                }

                                network.m_nodes[j].m_nodeMesh.vertices = vertexArray;
                            }
                        }
                    }

                    if (network.m_lanes == null) continue;
                    if (network.m_lanes.Length != 0 && affect == true) {
                        for (uint j = 0; j < network.m_lanes.Length; j++) {
                            if (network.m_lanes[j].m_verticalOffset == -0.3f) network.m_lanes[j].m_verticalOffset = -0.15f;
                        }
                    }
                }
            }
            Debug.Log("[30-15Curb] Applying Curb height modification finished.");
        }
    }
}
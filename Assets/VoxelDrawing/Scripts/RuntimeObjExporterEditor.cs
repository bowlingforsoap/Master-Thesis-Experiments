﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text;
using System.IO;

[CustomEditor(typeof(RuntimeObjExporter))]
public class RuntimeObjExporterEditor : Editor
{
    private static RuntimeObjExporter runtimeObjExporter;

    void OnEnable()
    {
        runtimeObjExporter = (RuntimeObjExporter)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Save Voxel Drawing"))
        {
            ObjExporter.DoExportWSubmeshes();
        }

        if (GUILayout.Button("Save Voxel Drawing (No Submeshes)"))
        {
            ObjExporter.DoExportWOSubmeshes();
        }

        GUILayout.Label("Remove:");
        if (GUILayout.Button("!!! Delete Voxel Drawing !!!")) {
            VoxelDrawing.VoxelDrawer.GetVoxelDrawer().EraseAllVoxels();
        }
    }

    // Adapted from Source: https://gist.github.com/MattRix/0522c27ee44c0fbbdf76d65de123eeff Author: Matt Rix	

    public class ObjExporterScript
    {
        private static int StartIndex = 0;

        public static void Start()
        {
            StartIndex = 0;
        }
        public static void End()
        {
            StartIndex = 0;
        }


        public static string MeshToString(MeshFilter mf, Transform t)
        {
            Vector3 s = t.localScale;
            Vector3 p = t.localPosition;
            Quaternion r = t.localRotation;


            int numVertices = 0;
            Mesh m = mf.sharedMesh;
            if (!m)
            {
                return "####Error####";
            }
            Material[] mats = mf.GetComponent<Renderer>().sharedMaterials;

            StringBuilder sb = new StringBuilder();

            foreach (Vector3 vv in m.vertices)
            {
                Vector3 v = t.TransformPoint(vv);
                numVertices++;
                sb.Append(string.Format("v {0} {1} {2}\n", v.x, v.y, -v.z));
            }
            sb.Append("\n");
            foreach (Vector3 nn in m.normals)
            {
                Vector3 v = r * nn;
                sb.Append(string.Format("vn {0} {1} {2}\n", -v.x, -v.y, v.z));
            }
            sb.Append("\n");
            foreach (Vector3 v in m.uv)
            {
                sb.Append(string.Format("vt {0} {1}\n", v.x, v.y));
            }
            for (int material = 0; material < m.subMeshCount; material++)
            {
                sb.Append("\n");
                sb.Append("usemtl ").Append(mats[material].name).Append("\n");
                sb.Append("usemap ").Append(mats[material].name).Append("\n");

                int[] triangles = m.GetTriangles(material);
                for (int i = 0; i < triangles.Length; i += 3)
                {
                    sb.Append(string.Format("f {0}/{0}/{0} {1}/{1}/{1} {2}/{2}/{2}\n",
                                            triangles[i] + 1 + StartIndex, triangles[i + 1] + 1 + StartIndex, triangles[i + 2] + 1 + StartIndex));
                }
            }

            StartIndex += numVertices;
            return sb.ToString();
        }
    }

    public class ObjExporter : ScriptableObject
    {
        private const string EXT = ".obj";

        // [MenuItem("File/Export/Wavefront OBJ")]
        public static void DoExportWSubmeshes()
        {
            DoExport(true);
        }

        // [MenuItem("File/Export/Wavefront OBJ (No Submeshes)")]
        public static void DoExportWOSubmeshes()
        {
            DoExport(false);
        }


        static void DoExport(bool makeSubmeshes)
        {
            if (runtimeObjExporter.savingPathAndName.Equals("")) {
                Debug.Log("Didn't Export Any Meshes; Please, specify the saving path in the VoxelDrawingManager's RuntimeObjExporter!");
                return;
            }

            if (Selection.gameObjects.Length == 0)
            {
                Debug.Log("Didn't Export Any Meshes; Nothing was selected!");
                return;
            }

            if (Selection.gameObjects[0].transform.childCount == 0) {
                Debug.Log("Didn't Export Any Meshes; No voxels were drawn! Did you select the correct object?");
                return;
            }

            // TODO: change to current shape name
            string meshName = runtimeObjExporter.shapeManager.GetActiveShape().name;
            Debug.Log("meshName: " + meshName);

            int count = 1;
            string fileName;
            while (true)
            {
                fileName = ConstructModelFileName(meshName, count);
                if (System.IO.File.Exists(fileName))
                {
                    count++;
                    continue;
                }

                break;
            }

            ObjExporterScript.Start();

            StringBuilder meshString = new StringBuilder();

            meshString.Append("#" + meshName + ".obj"
                              + "\n#" + System.DateTime.Now.ToLongDateString()
                              + "\n#" + System.DateTime.Now.ToLongTimeString()
                              + "\n#-------"
                              + "\n\n");

            Transform t = Selection.gameObjects[0].transform;

            Vector3 originalPosition = t.position;
            t.position = Vector3.zero;

            if (!makeSubmeshes)
            {
                meshString.Append("g ").Append(t.name).Append("\n");
            }
            meshString.Append(ProcessTransform(t, makeSubmeshes));

            WriteToFile(meshString.ToString(), fileName);

            t.position = originalPosition;

            ObjExporterScript.End();
            Debug.Log("Exported Mesh: " + fileName);

            runtimeObjExporter.SetShapeSaved(true);
        }

        static string ProcessTransform(Transform t, bool makeSubmeshes)
        {
            StringBuilder meshString = new StringBuilder();

            meshString.Append("#" + t.name
                              + "\n#-------"
                              + "\n");

            if (makeSubmeshes)
            {
                meshString.Append("g ").Append(t.name).Append("\n");
            }

            MeshFilter mf = t.GetComponent<MeshFilter>();
            if (mf != null)
            {
                meshString.Append(ObjExporterScript.MeshToString(mf, t));
            }

            for (int i = 0; i < t.childCount; i++)
            {
                meshString.Append(ProcessTransform(t.GetChild(i), makeSubmeshes));
            }

            return meshString.ToString();
        }

        static void WriteToFile(string s, string filename)
        {
            using (StreamWriter sw = new StreamWriter(filename))
            {
                sw.Write(s);
            }
        }

        /// <summary>count - number of files with the same name already in the directory.</summary>
        static string ConstructModelFileName(string meshName, int count) {
            return runtimeObjExporter.savingPathAndName + count + "_" + meshName + EXT;
        }
    }
}



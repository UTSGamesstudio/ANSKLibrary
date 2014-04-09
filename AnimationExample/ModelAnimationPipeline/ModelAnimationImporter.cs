using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using ModelAnimationLibrary;

// TODO: replace this with the type you want to import.
//using TImport = Microsoft.Xna.Framework.Content.Pipeline.Graphics.NodeContent;
using TImport = ModelAnimationPipeline.ANSKProcessContent;

namespace ModelAnimationPipeline
{
    /// <summary>
    /// This class will be instantiated by the XNA Framework Content Pipeline
    /// to import a file from disk into the specified type, TImport.
    /// 
    /// This should be part of a Content Pipeline Extension Library project.
    /// 
    /// TODO: change the ContentImporter attribute to specify the correct file
    /// extension, display name, and default processor for this importer.C:\Users\Joshua\Desktop\Work\SneakyPants3DStuff\AndAgain\TheGreatAlienEscape\src\ProjectSneakyGame\ProjectSneakyGameContent\NPC2.fbx
    /// </summary>

    [ContentImporter(".fbx", DisplayName = "Model Animation Importer", DefaultProcessor = "ModelAnimationProcessor")]
    //public class ModelAnimationImporterFbx : FbxImporter
    public class ModelAnimationImporterFbx : ContentImporter<TImport>
    {
        public override TImport Import(string filename, ContentImporterContext context)
        {
            //System.Diagnostics.Debugger.Launch();
            
            string fileInfo = System.IO.File.ReadAllText(filename);
            FbxImporter f = new FbxImporter();

            NodeContent c = f.Import(filename, context);

            ANSKFbxData data = new ANSKFbxData();
            data.LoadFbx(filename);

            /*for (int i = 0; i < c.Children.Count; i++)
            {
                MeshContent mesh = c.Children[i] as MeshContent;

                if (mesh != null)
                {
                    continue;
                }
            }*/

            return new TImport(c, data);
        }
    }

    /*[ContentImporter(".fbx", DisplayName = "Model Animation Importer", DefaultProcessor = "ModelAnimationProcessor")]
    public class ModelAnimationImporterFbx : Fbx//ContentImporter<TImport>
    {
        public override TImport Import(string filename, ContentImporterContext context)
        {
            //System.Diagnostics.Debugger.Launch();

            ContentIdentity identity = new ContentIdentity(filename, GetType().Name);
            AssimpImporter importer = new AssimpImporter();

            importer.AttachLogStream(new LogStream((msg, userData) => context.Logger.LogMessage(msg)));

            importer.SetConfig(new NormalSmoothingAngleConfig(0.66f));

            Scene scene = importer.ImportFile(filename, PostProcessSteps.FlipUVs |
                                                        PostProcessSteps.JoinIdenticalVertices |
                                                        PostProcessSteps.Triangulate |
                                                        PostProcessSteps.SortByPrimitiveType |
                                                        PostProcessSteps.FindInvalidData);

            NodeContent rootNode = new NodeContent
            {
                Name = scene.RootNode.Name,
                Identity = identity,
                Transform = AssimpToXna(scene.RootNode.Transform)
            };

            // Materials

            List<MaterialContent> materials = new List<MaterialContent>();

            foreach (Material sceneMaterial in scene.Materials)
            {
                TextureSlot diffuse = sceneMaterial.GetTexture(TextureType.Diffuse, 0);

                materials.Add(new BasicMaterialContent()
                {
                    Name = sceneMaterial.Name,
                    Identity = identity,
                    Texture = new ExternalReference<TextureContent>(diffuse.FilePath, identity)
                });
            }

            // Meshes

            Dictionary<Mesh, MeshContent> meshes = new Dictionary<Mesh, MeshContent>();

            foreach (Mesh sceneMesh in scene.Meshes)
            {
                if (!sceneMesh.HasVertices)
                    continue;

                MeshContent mesh = new MeshContent
                {
                    Name = sceneMesh.Name
                };

                foreach (Vector3D vert in sceneMesh.Vertices)
                    mesh.Positions.Add(new Vector3(vert.X, vert.Y, vert.Z));

                GeometryContent geom = new GeometryContent
                {
                    Name = string.Empty
                };

                //geom.Vertices.Positions.AddRange(mesh.Positions);
                geom.Vertices.AddRange(Enumerable.Range(0, sceneMesh.VertexCount));
                geom.Indices.AddRange(sceneMesh.GetIntIndices());

                if (sceneMesh.HasNormals)
                    geom.Vertices.Channels.Add(VertexChannelNames.Normal(), AssimpToXna(sceneMesh.Normals));

                for (int i = 0; i < sceneMesh.TextureCoordsChannelCount; i++)
                    geom.Vertices.Channels.Add(VertexChannelNames.TextureCoordinate(i), AssimpToXnaVector2(sceneMesh.GetTextureCoords(i)));

                mesh.Geometry.Add(geom);
                rootNode.Children.Add(mesh);
                meshes.Add(sceneMesh, mesh);
            }

            // Bones

            Dictionary<Node, BoneContent> bones = new Dictionary<Node, BoneContent>();
            IEnumerable<Node> hierarchyNodes = scene.RootNode.Children.SelectDeep(n => n.Children).ToList();

            foreach (Node node in hierarchyNodes)
            {
                BoneContent bone = new BoneContent
                {
                    Name = node.Name,
                    Transform = Matrix.Transpose(AssimpToXna(node.Transform))
                };

                if (node.Parent == scene.RootNode)
                    rootNode.Children.Add(bone);
                else
                {
                    BoneContent parent = bones[node.Parent];
                    parent.Children.Add(bone);
                }

                foreach (int meshIndex in node.MeshIndices)
                    meshes[scene.Meshes[meshIndex]].Name = node.Name;

                bones.Add(node, bone);
            }

            return rootNode;
        }

        private Matrix AssimpToXna(Matrix4x4 m)
        {
            return new Matrix(m.A1, m.A2, m.A3, m.A4,
                              m.B1, m.B2, m.B3, m.B4,
                              m.C1, m.C2, m.C3, m.C4,
                              m.D1, m.D2, m.D3, m.D4);
        }

        private Vector3[] AssimpToXna(Vector3D[] vector)
        {
            Vector3[] vs = new Vector3[vector.Length];

            for (int i = 0; i < vector.Length; i++)
                vs[i] = new Vector3(vector[i].X, vector[i].Y, vector[i].Z);

            return vs;
        }

        private Vector2[] AssimpToXnaVector2(Vector3D[] vectors)
        {
            Vector2[] vs = new Vector2[vectors.Length];

            for (int i = 0; i < vectors.Length; i++)
                vs[i] = new Vector2(vectors[i].X, vectors[i].Y);

            return vs;
        }
    }

    public static class EnumerableExtensions
    {
        /// <summary>
        /// Returns each element of a tree structure in heriarchical order.
        /// </summary>
        /// <typeparam name="T">The enumerated type.</typeparam>
        /// <param name="source">The enumeration to traverse.</param>
        /// <param name="selector">A function which returns the children of the element.</param>
        /// <returns>An IEnumerable whose elements are in tree structure heriarchical order.</returns>
        public static IEnumerable<T> SelectDeep<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> selector)
        {
            var stack = new Stack<T>(source.Reverse());
            while (stack.Count > 0)
            {
                // Return the next item on the stack.
                var item = stack.Pop();
                yield return item;

                // Get the children from this item.
                var children = selector(item);

                // If we have no children then skip it.
                if (children == null)
                    continue;

                // We're using a stack, so we need to push the
                // children on in reverse to get the correct order.
                foreach (var child in children.Reverse())
                    stack.Push(child);
            }
        }

        /// <summary>
        /// Returns an enumerable from a single element.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public static IEnumerable<T> AsEnumerable<T>(this T item)
        {
            yield return item;
        }
    }*/
}

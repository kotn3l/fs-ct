using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using SoulsFormats;
using System.Diagnostics;



namespace FLVERtoASCII
{
    class Conversion : IDisposable
    {
        //private List<string> ascii;
        List<FLVER2> model;

        public List<Thread> threads = new List<Thread>();
        public List<Thread> texThreads = new List<Thread>();

        private static string seperator = "-----------------------------";

        public List<FLVER2> Model
        {
            get { return model; }
            set { model = value; }
        }
        public Conversion(FLVER2 model)
        {
            Model = new List<FLVER2>();
            Model.Add(model);
            //ascii = new List<string>();
        }
        public Conversion(List<FLVER2> models)
        {
            Model = new List<FLVER2>(models);
            //Model.Add(model);
            //ascii = new List<string>();
        }
        public Conversion()
        {
            Model = new List<FLVER2>();
            //ascii = new List<string>();
        }

        public void switchPlatform(PLATFORM sourcePlatform, PLATFORM destinationPlatform, string dcxDirToSwitchPlatformOn)
        {
            if (sourcePlatform == destinationPlatform)
            {
                return;
            }
            string outPath = dcxDirToSwitchPlatformOn + "\\out";
            if (!Directory.Exists(outPath))
            {
                Directory.CreateDirectory(outPath);
            }
            string[] dcxT = Directory.GetFiles(dcxDirToSwitchPlatformOn, "*.dcx", SearchOption.AllDirectories);
            string[] bhdT = Directory.GetFiles(dcxDirToSwitchPlatformOn, "*.tpfbhd", SearchOption.AllDirectories);
            List<string> dcx = new List<string>(dcxT);
            List<string> tpf = new List<string>();
            List<string> bhd = new List<string>(bhdT);
            for (int i = 0; i < dcx.Count; i++)
            {
                if (Regex.Match(dcx[i], @"\..*").Value == ".tpf.dcx")
                {
                    tpf.Add(String.Copy(dcx[i]));
                    dcx.RemoveAt(i);
                    i = 0;
                }
            }
            List<BND4> archives = new List<BND4>();
            List<TPF> textures = new List<TPF>();


            string[] Platform_ToString = new string[] { "INTERROOT_win64", "INTERROOT_ps4" };
            //for (int i = 0; i < dcx.Length; i++)
            foreach (var item in dcx)
            {
                archives.Add(BND4.Read(item));
                for (int j = 0; j < archives.Last().Files.Count; j++)
                {
                    archives.Last().Files[j].Name = Regex.Replace(archives.Last().Files[j].Name, $"{Platform_ToString[(int)sourcePlatform]}", $"{Platform_ToString[(int)destinationPlatform]}");
                }
                string s = item.Remove(0, dcxDirToSwitchPlatformOn.Length);
                string file = Path.GetFileName(s);
                string combined = outPath + s;
                string dir = combined.Substring(0, combined.Length - file.Length);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                threads.Add(new Thread(() => archives.Last().Write(combined)));
                threads.Last().Start();
            }

            foreach (var item in tpf)
            {
                textures.Add(TPF.Read(item));

                foreach (var tex in textures.Last().Textures)
                {
                     tex.Bytes = tex.Headerize();
                }

                textures.Last().Platform = TPF.TPFPlatform.PC;
                //textures.Last().Textures[0].
                string s = item.Remove(0, dcxDirToSwitchPlatformOn.Length);
                string file = Path.GetFileName(s);
                string combined = outPath + s;
                string dir = combined.Substring(0, combined.Length - file.Length);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                threads.Add(new Thread(() => textures.Last().Write(combined)));
                threads.Last().Start();
            }

            while (threads.Any(x => x.IsAlive))
            {

            }
        }
        private static bool HasCharsInRange(string text, int min, int max)
        {
            return text.Any(e => e >= min && e <= max);
        }

        public void WriteFLVERtoASCII(string outPath, string fileName, bool bones = false, bool addRoot = false, int index = 0, Dictionary<int, List<MATBIN>> material = null)
        {
            if (File.Exists(Path.Combine(outPath, fileName + ".ascii")))
            {
                return;
            }
            List<string> ascii = new List<string>();
            Dictionary<string, List<string>> texturestxt = new Dictionary<string, List<string>>();
            List<string> texturestxtFile = new List<string>();
            texturestxtFile.Add(fileName);
            texturestxtFile.Add(seperator);

            int plusBone = 0;
            if (bones)
            {

                if (addRoot)
                {
                    plusBone = 1;
                }
                var boneMatrices = new Matrix4x4[Model[index].Bones.Count + plusBone];
                ascii.Add((Model[index].Bones.Count + plusBone).ToString()); //Bone count
                if (addRoot)
                {
                    ascii.Add("root");
                    ascii.Add("-1");
                    ascii.Add("0 0 0 0 0 0 1");
                }

                for (int i = 0; i < Model[index].Bones.Count; i++) //Bone names, parents, xyz
                {
                    if (HasCharsInRange(Model[index].Bones[i].Name, 0x30A0, 0x30FF) || HasCharsInRange(Model[index].Bones[i].Name, 0x4E00, 0x9FFF))
                    {
                        ascii.Add(Model[index].Bones[i].Name + i);
                    }
                    else ascii.Add(Model[index].Bones[i].Name);
                    short pIndex = Model[index].Bones[i].ParentIndex;
                    Matrix4x4 translation = Matrix4x4.Identity;
                    if (true)
                    {
                        translation.M11 *= -1; //FLIP BONES along Z axis
                    }
                    if (pIndex != -1)
                    {
                        translation = boneMatrices[pIndex];
                    }

                    boneMatrices[i] = Model[index].Bones[i].ComputeLocalTransform() * translation;
                    ascii.Add((pIndex + plusBone).ToString());
                    ascii.Add(boneMatrices[i].M41.ToString("0.##########") + " " +
                              boneMatrices[i].M42.ToString("0.##########") + " " +
                              boneMatrices[i].M43.ToString("0.##########"));
                }
            }
            else
            {
                ascii.Add("0");
            }
            ascii.Add(Model[index].Meshes.Count.ToString());
            for (int i = 0; i < Model[index].Meshes.Count; i++)
            {
                if (!texturestxt.ContainsKey(Model[index].Materials[Model[index].Meshes[i].MaterialIndex].Name))
                {
                    texturestxt.Add(Model[index].Materials[Model[index].Meshes[i].MaterialIndex].Name, new List<string>());
                }
                for (int l = 0; l < material[Model[index].GetHashCode()][Model[index].Meshes[i].MaterialIndex].Samplers.Count; l++)
                {
                    if (material[Model[index].GetHashCode()][Model[index].Meshes[i].MaterialIndex].Samplers[l].Path != "")
                    {
                        if (!texturestxt[Model[index].Materials[Model[index].Meshes[i].MaterialIndex].Name].Contains("\t" + Path.GetFileName(material[Model[index].GetHashCode()][Model[index].Meshes[i].MaterialIndex].Samplers[l].Path)))
                        {
                            texturestxt[Model[index].Materials[Model[index].Meshes[i].MaterialIndex].Name].Add("\t" + Path.GetFileName(material[Model[index].GetHashCode()][Model[index].Meshes[i].MaterialIndex].Samplers[l].Path));
                        }
                    } //else texturestxt[Model[index].Materials[Model[index].Meshes[i].MaterialIndex].Name].Add("\tEMPTY_SLOT");

                }
                //texturestxt.Add(seperator);

                int Tex = 0;
                for (int k = 0; k < material[Model[index].GetHashCode()].Count; k++)
                {
                    Tex += material[Model[index].GetHashCode()][Model[index].Meshes[i].MaterialIndex].Samplers.Count;
                }
                if (Model[index].Materials[Model[index].Meshes[i].MaterialIndex].Name[0] == '#' ||
                    Model[index].Materials[Model[index].Meshes[i].MaterialIndex].Name.Contains('#'))
                {
                    ascii.Add(Regex.Replace(Model[index].Materials[Model[index].Meshes[i].MaterialIndex].Name, "#", "")); //mesh name
                } else ascii.Add(Model[index].Materials[Model[index].Meshes[i].MaterialIndex].Name); 
                ascii.Add(Model[index].Meshes[i].Vertices[0].UVs.Count.ToString()); //mesh UV channel count
                ascii.Add(Tex.ToString()); //tex count
                for (int k = 0; k < material[Model[index].GetHashCode()].Count; k++)
                {
                    for (int l = 0; l < material[Model[index].GetHashCode()][Model[index].Meshes[i].MaterialIndex].Samplers.Count; l++)
                    {
                        ascii.Add(material[Model[index].GetHashCode()][Model[index].Meshes[i].MaterialIndex].Samplers[l].Type);
                        if (Path.GetFileName(material[Model[index].GetHashCode()][Model[index].Meshes[i].MaterialIndex].Samplers[l].Path) == "" ||
                                             material[Model[index].GetHashCode()][Model[index].Meshes[i].MaterialIndex].Samplers[l].Path == "")
                        {
                            ascii.Add("UNK");
                        }
                        else
                        {
                            ascii.Add(Path.GetFileName(material[Model[index].GetHashCode()][Model[index].Meshes[i].MaterialIndex].Samplers[l].Path));
                        }
                    }
                }
                ascii.Add(Model[index].Meshes[i].Vertices.Count.ToString()); //mesh vertices count
                for (int j = 0; j < Model[index].Meshes[i].Vertices.Count; j++) //vertices
                {
                    //vert pos
                    ascii.Add(-Model[index].Meshes[i].Vertices[j].Position.X + " " +
                              Model[index].Meshes[i].Vertices[j].Position.Y + " " +
                              Model[index].Meshes[i].Vertices[j].Position.Z);
                    //vert norm
                    ascii.Add(-Model[index].Meshes[i].Vertices[j].Normal.X + " " +
                              Model[index].Meshes[i].Vertices[j].Normal.Y + " " +
                              Model[index].Meshes[i].Vertices[j].Normal.Z);
                    //vert colors
                    ascii.Add((Model[index].Meshes[i].Vertices[j].Colors[0].R * 255) + " " +
                              (Model[index].Meshes[i].Vertices[j].Colors[0].G * 255) + " " +
                              (Model[index].Meshes[i].Vertices[j].Colors[0].B * 255) + " " +
                              (Model[index].Meshes[i].Vertices[j].Colors[0].A * 255));
                    //vert uvs
                    for (int k = 0; k < Model[index].Meshes[i].Vertices[j].UVs.Count; k++)
                    {
                        ascii.Add(Model[index].Meshes[i].Vertices[j].UVs[k].X + " " +
                              Model[index].Meshes[i].Vertices[j].UVs[k].Y);
                    }
                    //vert bone indices
                    string indices = "";
                    for (int k = 0; k < 3; k++)
                    {
                        indices += Model[index].Meshes[i].Vertices[j].BoneIndices[k] + plusBone + " ";
                        //IndiceIndex = k;
                    }
                    indices += Model[index].Meshes[i].Vertices[j].BoneIndices[3] + plusBone;
                    ascii.Add(indices);

                    string weights = "";
                    int WeightIndex = 0;
                    for (int k = 0; k < Model[index].Meshes[i].Vertices[j].BoneWeights.Length - 1; k++)
                    {
                        weights += Model[index].Meshes[i].Vertices[j].BoneWeights[k] + " ";
                        WeightIndex = k;
                    }
                    weights += Model[index].Meshes[i].Vertices[j].BoneWeights[WeightIndex + 1];
                    ascii.Add(weights);
                }
                int FaceCount = Model[index].Meshes[i].FaceSets[0].Indices.Count;
                ascii.Add((FaceCount / 3).ToString());
                for (int j = 0; j < FaceCount; j++)
                {
                    ascii.Add(Model[index].Meshes[i].FaceSets[0].Indices[j] + " " +
                              Model[index].Meshes[i].FaceSets[0].Indices[j + 2] + " " +
                              Model[index].Meshes[i].FaceSets[0].Indices[j + 1]); //j+1 is last because of flipping
                    j++;
                    j++;
                }

            }

            foreach (var item in texturestxt)
            {
                texturestxtFile.Add(item.Key);
                foreach (string text in item.Value)
                {
                    texturestxtFile.Add(text);
                }
                texturestxtFile.Add(seperator);
            }
            File.WriteAllLines(outPath + "\\" + fileName + "_tex.txt", texturestxtFile);

            using (StreamWriter outputFile = new StreamWriter(Path.Combine(outPath, fileName + ".ascii")))
            {
                foreach (string line in ascii)
                    outputFile.WriteLine(line);
            }

            ascii.Clear();

        }
        public void WriteFLVERtoASCIIInOne(string outPath, string fileName, Dictionary<int, List<Matrix4x4>> transform, Dictionary<int, List<MATBIN>> material, bool bones = false, bool addRoot = false)
        {
            if (File.Exists(Path.Combine(outPath, fileName + ".ascii")))
            {
                return;
            }

            List<string> ascii = new List<string>();
            Dictionary<int, Matrix4x4> transformOne = new Dictionary<int, Matrix4x4>();
            Dictionary<int, List<Matrix4x4>> transformMultpile = new Dictionary<int, List<Matrix4x4>>();
            Dictionary<string, List<string>> texturestxt = new Dictionary<string, List<string>>();
            List<string> texturestxtFile = new List<string>();
            texturestxtFile.Add(fileName);
            texturestxtFile.Add(seperator);
            int plusBone = 0;
            if (addRoot)
            {
                plusBone = 1;
            }

            int boneSum = 0;
            uint MeshSum = 0;
            uint objectCopies = 0;
            for (int i = 0; i < Model.Count; i++)
            {
                if (!transform.ContainsKey(Model[i].GetHashCode()))
                {
                    continue;
                }
                if (transform[Model[i].GetHashCode()].Count == 1)
                {
                    transformOne.Add(Model[i].GetHashCode(), transform[Model[i].GetHashCode()][0]);
                }
                else
                {
                    transformMultpile.Add(Model[i].GetHashCode(), transform[Model[i].GetHashCode()]);
                }
                for (int z = 0; z < transform[Model[i].GetHashCode()].Count; z++)
                {
                    boneSum += model[i].Bones.Count;
                    objectCopies++;
                }
                
            }
            for (int e = 0; e < Model.Count; e++)
            {
                if (!transform.ContainsKey(Model[e].GetHashCode()))
                {
                    continue;
                }
                for (int z = 0; z < transform[Model[e].GetHashCode()].Count; z++)
                {
                    for (int i = 0; i < Model[e].Meshes.Count; i++) //overall meshes
                    {
                        MeshSum++;
                    }
                }
                
            }
            if (bones)
            {
                ascii.Add((boneSum + plusBone).ToString());
            }
            else ascii.Add("0");
            if (addRoot)
            {
                ascii.Add("root");
                ascii.Add("-1");
                ascii.Add("0 0 0 0 0 0 1");
            }
            for (int y = 0; y < Model.Count; y++)
            {
                if (!transform.ContainsKey(Model[y].GetHashCode()))
                {
                    continue;
                }
                for (int z = 0; z < transform[Model[y].GetHashCode()].Count; z++)
                {
                    var boneMatrices = new Matrix4x4[Model[y].Bones.Count + plusBone];
                    List<int> countIndex = new List<int>();
                    for (int i = 0; i < Model[y].Bones.Count; i++) //Bone names, parents, xyz
                    {
                        ascii.Add(geometry.FirstOrDefault(x => x.Value == Model[y]).Key + Model[y].Bones[i].Name);
                        int pIndex = Model[y].Bones[i].ParentIndex;
                        Matrix4x4 translation = Matrix4x4.Identity;
                        if (true)
                        {
                            translation.M11 *= -1; //FLIP BONES along Z axis
                        }
                        if (pIndex != -1)
                        {
                            translation = boneMatrices[pIndex];
                        }
                        boneMatrices[i] = Model[y].Bones[i].ComputeLocalTransform() * translation * transform[Model[y].GetHashCode()][z];

                        if (pIndex != -1)
                        {
                            //pIndex += prevBones;
                        }
                        ascii.Add((pIndex + plusBone).ToString());
                        ascii.Add(boneMatrices[i].M41.ToString("0.##########") + " " +
                                  boneMatrices[i].M42.ToString("0.##########") + " " +
                                  boneMatrices[i].M43.ToString("0.##########"));
                    }

                }
            }

            ascii.Add(MeshSum.ToString());
            for (int y = 0; y < Model.Count; y++)
            {
                if (!transform.ContainsKey(Model[y].GetHashCode())) //if model is not even on the map, skip
                {
                    continue;
                }
                for (int z = 0; z < transform[Model[y].GetHashCode()].Count; z++)
                {
                    for (int i = 0; i < Model[y].Meshes.Count; i++)
                    {
                        if (!texturestxt.ContainsKey(Model[y].Materials[Model[y].Meshes[i].MaterialIndex].Name))
                        {
                            texturestxt.Add(Model[y].Materials[Model[y].Meshes[i].MaterialIndex].Name, new List<string>());
                        }
                        if (material[Model[y].GetHashCode()].Count > 0)
                        {
                            for (int l = 0; l < material[Model[y].GetHashCode()][Model[y].Meshes[i].MaterialIndex].Samplers.Count; l++)
                            {
                                if (material[Model[y].GetHashCode()][Model[y].Meshes[i].MaterialIndex].Samplers[l].Path != "")
                                {
                                    if (!texturestxt[Model[y].Materials[Model[y].Meshes[i].MaterialIndex].Name].Contains("\t" + Path.GetFileName(material[Model[y].GetHashCode()][Model[y].Meshes[i].MaterialIndex].Samplers[l].Path)))
                                    {
                                        texturestxt[Model[y].Materials[Model[y].Meshes[i].MaterialIndex].Name].Add("\t" + Path.GetFileName(material[Model[y].GetHashCode()][Model[y].Meshes[i].MaterialIndex].Samplers[l].Path));
                                    }
                                }
                                //else texturestxt[Model[y].Materials[Model[y].Meshes[i].MaterialIndex].Name].Add("\tEMPTY_SLOT");
                            }
                        }
                        
                        //texturestxt.Add(seperator);


                        int Tex = 0;
                        for (int k = 0; k < material[Model[y].GetHashCode()].Count; k++)
                        {
                            Tex += material[Model[y].GetHashCode()][Model[y].Meshes[i].MaterialIndex].Samplers.Count;
                        }
                        if (Model[y].Materials[Model[y].Meshes[i].MaterialIndex].Name[0] == '#')
                        {
                            ascii.Add(Regex.Replace(Model[y].Materials[Model[y].Meshes[i].MaterialIndex].Name, "#", "")); //mesh name
                        }
                        else ascii.Add(Model[y].Materials[Model[y].Meshes[i].MaterialIndex].Name);
                        ascii.Add(Model[y].Meshes[i].Vertices[0].UVs.Count.ToString()); //mesh UV channel count
                        ascii.Add(Tex.ToString()); //tex count
                        for (int k = 0; k < material[Model[y].GetHashCode()].Count; k++)
                        {
                            for (int l = 0; l < material[Model[y].GetHashCode()][Model[y].Meshes[i].MaterialIndex].Samplers.Count; l++)
                            {
                                ascii.Add(material[Model[y].GetHashCode()][Model[y].Meshes[i].MaterialIndex].Samplers[l].Type);
                                if (Path.GetFileName(material[Model[y].GetHashCode()][Model[y].Meshes[i].MaterialIndex].Samplers[l].Path) == "" || material[Model[y].GetHashCode()][Model[y].Meshes[i].MaterialIndex].Samplers[l].Path == "")
                                {
                                    ascii.Add("UNK");
                                }
                                else
                                {
                                    ascii.Add(Path.GetFileName(material[Model[y].GetHashCode()][Model[y].Meshes[i].MaterialIndex].Samplers[l].Path));
                                }
                            }
                        }

                        ascii.Add(Model[y].Meshes[i].Vertices.Count.ToString()); //mesh vertices count
                        for (int j = 0; j < Model[y].Meshes[i].Vertices.Count; j++) //vertices
                        {
                            Vector3 transformedPos = Vector3.Transform(new Vector3(-Model[y].Meshes[i].Vertices[j].Position.X, 
                                                                                    Model[y].Meshes[i].Vertices[j].Position.Y, 
                                                                                    Model[y].Meshes[i].Vertices[j].Position.Z), 
                                                                                    transform[Model[y].GetHashCode()][z]);
                            Vector3 transformedNormal = Vector3.TransformNormal(new Vector3(-Model[y].Meshes[i].Vertices[j].Normal.X, 
                                                                                            Model[y].Meshes[i].Vertices[j].Normal.Y, 
                                                                                            Model[y].Meshes[i].Vertices[j].Normal.Z), 
                                                                                            transform[Model[y].GetHashCode()][z]);

                            ascii.Add(transformedPos.X + " " +
                                      transformedPos.Y + " " +
                                      transformedPos.Z);
                            //vert norm

                            ascii.Add(transformedNormal.X + " " +
                                      transformedNormal.Y + " " +
                                      transformedNormal.Z);
                            //vert colors
                            /*ascii.Add((Model[y].Meshes[i].Vertices[j].Colors[0].R * 255) + " " +
                                      (Model[y].Meshes[i].Vertices[j].Colors[0].G * 255) + " " +
                                      (Model[y].Meshes[i].Vertices[j].Colors[0].B * 255) + " " +
                                      (Model[y].Meshes[i].Vertices[j].Colors[0].A * 255));*/
                            ascii.Add(255 + " " +
                                      255 + " " +
                                      255 + " " +
                                      0);
                            //vert uvs
                            for (int k = 0; k < Model[y].Meshes[i].Vertices[j].UVs.Count; k++)
                            {
                                ascii.Add(Model[y].Meshes[i].Vertices[j].UVs[k].X + " " +
                                      Model[y].Meshes[i].Vertices[j].UVs[k].Y);
                            }
                            //vert bone indices
                            string indices = "";
                            for (int k = 0; k < 3; k++)
                            {
                                indices += Model[y].Meshes[i].Vertices[j].BoneIndices[k] + plusBone + " ";
                                //IndiceIndex = k;
                            }
                            indices += Model[y].Meshes[i].Vertices[j].BoneIndices[3] + plusBone;
                            ascii.Add(indices);

                            string weights = "";
                            int WeightIndex = 0;
                            for (int k = 0; k < Model[y].Meshes[i].Vertices[j].BoneWeights.Length - 1; k++)
                            {
                                weights += Model[y].Meshes[i].Vertices[j].BoneWeights[k] + " ";
                                WeightIndex = k;
                            }
                            weights += Model[y].Meshes[i].Vertices[j].BoneWeights[WeightIndex + 1];
                            ascii.Add(weights);
                        }
                        int FaceCount = Model[y].Meshes[i].FaceSets[0].Indices.Count;
                        ascii.Add((FaceCount / 3).ToString());
                        for (int j = 0; j < FaceCount; j++)
                        {
                            ascii.Add(Model[y].Meshes[i].FaceSets[0].Indices[j] + " " +
                                      Model[y].Meshes[i].FaceSets[0].Indices[j + 2] + " " +
                                      Model[y].Meshes[i].FaceSets[0].Indices[j + 1]); //j+1 is last because of flipping
                            j++;
                            j++;
                        }

                    }
                    //texturestxt.Add(seperator);
                }
            }
            foreach (var item in texturestxt)
            {
                texturestxtFile.Add(item.Key);
                foreach (string text in item.Value)
                {
                    texturestxtFile.Add(text);
                }
                texturestxtFile.Add(seperator);
            }
            File.WriteAllLines(outPath + "\\" + fileName + "_tex.txt", texturestxtFile);
            texturestxt.Clear();
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(outPath, fileName + ".ascii")))
            {
                foreach (string line in ascii)
                    outputFile.WriteLine(line);
            }
            ascii.Clear();

        }
        public void WriteFLVERtoASCIIInOneCustomBones(string outPath, string fileName, List<FLVER.Bone> cbones, List<Matrix4x4> transform, Dictionary<int, List<MATBIN>> material, List<bool> overrideWeights, bool bones = false, bool addRoot = false)
        {
            if (File.Exists(Path.Combine(outPath, fileName + ".ascii")))
            {
                return;
            }
            List<string> ascii = new List<string>();
            Dictionary<string, List<string>> texturestxt = new Dictionary<string, List<string>>();
            List<string> texturestxtFile = new List<string>();
            texturestxtFile.Add(fileName);
            texturestxtFile.Add(seperator);
            int plusBone = 0;
            if (bones)
            {
                if (addRoot)
                {
                    plusBone = 1;
                }
                ascii.Add((cbones.Count+plusBone).ToString()); //Bone count

                if (addRoot)
                {
                    ascii.Add("root");
                    ascii.Add("-1");
                    ascii.Add("0 0 0 0 0 0 1");
                }
                var boneMatrices = new Matrix4x4[cbones.Count + plusBone];
                for (int i = 0; i < cbones.Count; i++) //Bone names, parents, xyz
                {
                    if (HasCharsInRange(cbones[i].Name, 0x30A0, 0x30FF) || HasCharsInRange(cbones[i].Name, 0x4E00, 0x9FFF))
                    {
                        ascii.Add(cbones[i].Name + i);
                    } else ascii.Add(cbones[i].Name);
                    short pIndex = cbones[i].ParentIndex;
                    Matrix4x4 translation = Matrix4x4.Identity;
                    if (true)
                    {
                        translation.M11 *= -1; //FLIP BONES along Z axis
                    }
                    if (pIndex != -1)
                    {
                        translation = boneMatrices[pIndex];
                    }
                    boneMatrices[i] = cbones[i].ComputeLocalTransform() * translation;
                    ascii.Add((pIndex + plusBone).ToString());
                    ascii.Add(boneMatrices[i].M41.ToString("0.##########") + " " +
                              boneMatrices[i].M42.ToString("0.##########") + " " +
                              boneMatrices[i].M43.ToString("0.##########"));
                }
            }
            else
            {
                ascii.Add("0");
            }
            
            uint MeshSum = 0;
            for (int index = 0; index < Model.Count; index++)
            {
                for (int i = 0; i < Model[index].Meshes.Count; i++) //overall meshes
                {
                    MeshSum++;
                }

            }
            ascii.Add(MeshSum.ToString());
            for (int index = 0; index < Model.Count; index++)
            {
                for (int i = 0; i < Model[index].Meshes.Count; i++)
                {
                    if (!texturestxt.ContainsKey(Model[index].Materials[Model[index].Meshes[i].MaterialIndex].Name))
                    {
                        texturestxt.Add(Model[index].Materials[Model[index].Meshes[i].MaterialIndex].Name, new List<string>());
                    }
                    for (int l = 0; l < material[Model[index].GetHashCode()][Model[index].Meshes[i].MaterialIndex].Samplers.Count; l++)
                    {
                        if (material[Model[index].GetHashCode()][Model[index].Meshes[i].MaterialIndex].Samplers[l].Path != "")
                        {
                            if (!texturestxt[Model[index].Materials[Model[index].Meshes[i].MaterialIndex].Name].Contains("\t" + Path.GetFileName(material[Model[index].GetHashCode()][Model[index].Meshes[i].MaterialIndex].Samplers[l].Path)))
                            {
                                texturestxt[Model[index].Materials[Model[index].Meshes[i].MaterialIndex].Name].Add("\t" + Path.GetFileName(material[Model[index].GetHashCode()][Model[index].Meshes[i].MaterialIndex].Samplers[l].Path));
                            }
                        }
                        //else texturestxt[Model[index].Materials[Model[index].Meshes[i].MaterialIndex].Name].Add("\tEMPTY_SLOT");

                    }
                    //texturestxt.Add(seperator);

                    int Tex = 0;
                    for (int k = 0; k < material[Model[index].GetHashCode()].Count; k++)
                    {
                        Tex += material[Model[index].GetHashCode()][Model[index].Meshes[i].MaterialIndex].Samplers.Count;
                    }
                    if (Model[index].Materials[Model[index].Meshes[i].MaterialIndex].Name[0] == '#')
                    {
                        ascii.Add(Regex.Replace(Model[index].Materials[Model[index].Meshes[i].MaterialIndex].Name, "#", "")); //mesh name
                    }
                    else ascii.Add(Model[index].Materials[Model[index].Meshes[i].MaterialIndex].Name);
                    ascii.Add(Model[index].Meshes[i].Vertices[0].UVs.Count.ToString()); //mesh UV channel count
                    ascii.Add(Tex.ToString()); //tex count
                    for (int k = 0; k < material[Model[index].GetHashCode()].Count; k++)
                    {
                        for (int l = 0; l < material[Model[index].GetHashCode()][Model[index].Meshes[i].MaterialIndex].Samplers.Count; l++)
                        {
                            ascii.Add(material[Model[index].GetHashCode()][Model[index].Meshes[i].MaterialIndex].Samplers[l].Type);
                            if (Path.GetFileName(material[Model[index].GetHashCode()][Model[index].Meshes[i].MaterialIndex].Samplers[l].Path) == "" ||
                                                 material[Model[index].GetHashCode()][Model[index].Meshes[i].MaterialIndex].Samplers[l].Path == "")
                            {
                                ascii.Add("UNK");
                            }
                            else
                            {
                                ascii.Add(Path.GetFileName(material[Model[index].GetHashCode()][Model[index].Meshes[i].MaterialIndex].Samplers[l].Path));
                            }
                        }
                    }

                    ascii.Add(Model[index].Meshes[i].Vertices.Count.ToString()); //mesh vertices count
                    for (int j = 0; j < Model[index].Meshes[i].Vertices.Count; j++) //vertices
                    {
                        Vector3 transformedPos = Vector3.Transform(new Vector3(-Model[index].Meshes[i].Vertices[j].Position.X,
                                                                                   Model[index].Meshes[i].Vertices[j].Position.Y,
                                                                                   Model[index].Meshes[i].Vertices[j].Position.Z),
                                                                                   transform[index]);
                        Vector3 transformedNormal = Vector3.TransformNormal(new Vector3(-Model[index].Meshes[i].Vertices[j].Normal.X,
                                                                                        Model[index].Meshes[i].Vertices[j].Normal.Y,
                                                                                        Model[index].Meshes[i].Vertices[j].Normal.Z),
                                                                                        transform[index]);

                        //vert pos
                        ascii.Add(transformedPos.X + " " +
                                  transformedPos.Y + " " +
                                  transformedPos.Z);
                        //vert norm
                        ascii.Add(transformedNormal.X + " " +
                                  transformedNormal.Y + " " +
                                  transformedNormal.Z);
                        //vert colors
                        ascii.Add((Model[index].Meshes[i].Vertices[j].Colors[0].R * 255) + " " +
                                  (Model[index].Meshes[i].Vertices[j].Colors[0].G * 255) + " " +
                                  (Model[index].Meshes[i].Vertices[j].Colors[0].B * 255) + " " +
                                  (Model[index].Meshes[i].Vertices[j].Colors[0].A * 255));
                        //vert uvs
                        for (int k = 0; k < Model[index].Meshes[i].Vertices[j].UVs.Count; k++)
                        {
                            ascii.Add(Model[index].Meshes[i].Vertices[j].UVs[k].X + " " +
                                  Model[index].Meshes[i].Vertices[j].UVs[k].Y);
                        }
                        //vert bone indices
                        string indices = "";
                        for (int k = 0; k < 3; k++)
                        {
                            int t = cbones.FindIndex(x => x.Name == Model[index].Bones[Model[index].Meshes[i].Vertices[j].BoneIndices[k]].Name);
                                    indices += (t + plusBone) + " ";
                        }
                        int temp = cbones.FindIndex(x => x.Name == Model[index].Bones[Model[index].Meshes[i].Vertices[j].BoneIndices[3]].Name);
                        indices += (temp + plusBone);
                        ascii.Add(indices);

                        string weights = "";
                        int WeightIndex = 0;
                        for (int k = 0; k < Model[index].Meshes[i].Vertices[j].BoneWeights.Length - 1; k++)
                        {
                            weights += Model[index].Meshes[i].Vertices[j].BoneWeights[k] + " ";
                            WeightIndex = k;
                        }
                        weights += Model[index].Meshes[i].Vertices[j].BoneWeights[WeightIndex + 1];
                        if (overrideWeights[index])
                        {
                            ascii.Add("1 1 1 1");
                        }
                        else ascii.Add(weights);
                    }


                    int FaceCount = Model[index].Meshes[i].FaceSets[0].Indices.Count;
                    ascii.Add((FaceCount / 3).ToString());
                    for (int j = 0; j < FaceCount; j++)
                    {
                        ascii.Add(Model[index].Meshes[i].FaceSets[0].Indices[j] + " " +
                                  Model[index].Meshes[i].FaceSets[0].Indices[j + 2] + " " +
                                  Model[index].Meshes[i].FaceSets[0].Indices[j + 1]); //j+1 is last because of flipping
                        j++;
                        j++;
                    }

                }
            }

            foreach (var item in texturestxt)
            {
                texturestxtFile.Add(item.Key);
                foreach (string text in item.Value)
                {
                    texturestxtFile.Add(text);
                }
                texturestxtFile.Add(seperator);
            }
            File.WriteAllLines(outPath + "\\" + fileName + "_tex.txt", texturestxtFile);

            using (StreamWriter outputFile = new StreamWriter(Path.Combine(outPath, fileName + ".ascii")))
            {
                foreach (string line in ascii)
                    outputFile.WriteLine(line);
            }

            ascii.Clear();
            
        }
        public void chrbndFolder(string inPath, string outPath, string erdir, bool textures = true)
        {
            string[] tomb = Directory.GetFiles(inPath, "*.chrbnd");
            string[] tombP = Directory.GetFiles(inPath, "*.partsbnd");
            string[] texs = Directory.GetFiles(inPath, "*.texbnd");
            string[] flvers = Directory.GetFiles(inPath, "*.flver");
            List<string> filenames = new List<string>();
            List<string> filenamesP = new List<string>();
            List<string> filenamesTex = new List<string>();
            List<BND4> chrbnds = new List<BND4>();
            //List<BND4> partbnds = new List<BND4>();
            List<BND4> texbnds = new List<BND4>();

            if (!Directory.Exists(outPath))
            {
                Directory.CreateDirectory(outPath);
            }

            Dictionary<int, List<MATBIN>> materials = new Dictionary<int, List<MATBIN>>();
            BND4 matbnd = BND4.Read(erdir + "//material//allmaterial.matbinbnd");
            string matPathFirst = "N:\\GR\\data\\INTERROOT_win64\\material\\matbin";
            

            for (int i = 0; i < flvers.Length; i++)
            {
                Model.Add(FLVER2.Read(flvers[i]));
                filenames.Add(Path.GetFileNameWithoutExtension(flvers[i]));
            }
            for (int i = 0; i < tombP.Length; i++)
            {
                BND4 partbnd = BND4.Read(tombP[i]);
                for (int j = 0; j < partbnd.Files.Count; j++)
                {
                    if (Path.GetExtension(partbnd.Files[j].Name).ToLower() == ".flver")
                    {
                        Model.Add(FLVER2.Read(partbnd.Files[j].Bytes));
                        filenamesP.Add(Path.GetFileNameWithoutExtension(partbnd.Files[j].Name));
                    }
                    else if (Path.GetExtension(partbnd.Files[j].Name).ToLower() == ".tpf")
                    {
                        Directory.CreateDirectory(outPath + "//" + Path.GetFileName(partbnd.Files[j].Name));
                        texture(TPF.Read(partbnd.Files[j].Bytes), outPath + "//" + Path.GetFileName(partbnd.Files[j].Name));
                    }
                }


            }
            for (int i = 0; i < tomb.Length; i++)
            {
                chrbnds.Add(BND4.Read(tomb[i]));
                for (int j = 0; j < chrbnds[i].Files.Count; j++)
                {
                    if (Path.GetExtension(chrbnds[i].Files[j].Name).ToLower() == ".flver")
                    {
                        Model.Add(FLVER2.Read(chrbnds[i].Files[j].Bytes));
                        filenames.Add(Path.GetFileNameWithoutExtension(chrbnds[i].Files[j].Name));
                    }
                    else if (textures && Path.GetExtension(chrbnds[i].Files[j].Name).ToLower() == ".tpf")
                    {
                        Directory.CreateDirectory(outPath + "//" + Path.GetFileName(chrbnds[i].Files[j].Name));
                        texture(TPF.Read(chrbnds[i].Files[j].Bytes), outPath + "//" + Path.GetFileName(chrbnds[i].Files[j].Name));
                    }
                }


            }
            if (textures)
            {
                for (int i = 0; i < texs.Length; i++)
                {
                    texbnds.Add(BND4.Read(texs[i]));
                    for (int j = 0; j < texbnds[i].Files.Count; j++)
                    {
                        if (Path.GetExtension(texbnds[i].Files[j].Name).ToLower() == ".tpf")
                        {
                            Directory.CreateDirectory(outPath + "//" + Path.GetFileName(texbnds[i].Files[j].Name));
                            texture(TPF.Read(texbnds[i].Files[j].Bytes), outPath + "//" + Path.GetFileName(texbnds[i].Files[j].Name));
                        }
                    }
                }
            }
            convertTextures(ref materials, ref matbnd, erdir, matPathFirst, outPath, false);
            for (int i = 0; i < Model.Count; i++)
            {
                WriteFLVERtoASCII(outPath, filenames[i], true, true, i, materials);
            }
            
        }
        public void texture(TPF text, string outPath)
        {
            for (int i = 0; i < text.Textures.Count; i++)
            {
                try
                {
                    if (!Directory.Exists(outPath))
                    {
                        Directory.CreateDirectory(outPath);
                    }
                    if (!File.Exists(Path.Combine(outPath, text.Textures[i].Name + ".dds")))
                    {
                        File.WriteAllBytes(Path.Combine(outPath, text.Textures[i].Name + ".dds"), text.Textures[i].Bytes);
                    }
                }
                catch (IOException e)
                {
                    //fails.Add(e.Message);
                    continue;
                }
                catch (Exception a)
                {
                    continue;
                    //fails.Add("UNKOWN ERROR: " + a.Message);
                }
                
            }

        }
        private void assembleMasterRig(List<FLVER.Bone> full, FLVER2 merge, int j)
        {
            for (int k = 0; k < merge.Bones.Count; k++) //iterate through our bones
            {
                if (merge.Bones[k].ParentIndex == j) //if any other childbone references our bone by its PID
                {
                    int original = merge.Bones[k].ParentIndex; //save PID
                    merge.Bones[k].ParentIndex = (short)full.FindIndex(x => x.Name == merge.Bones[j].Name); //change the PID of that childbone to the same as in MASTER
                    if (full.Any(x => x.Name == merge.Bones[k].Name)) //if that childbone is also in the MASTER
                    {
                        int minus = -1;
                        minus = full.FindIndex(x => x.Name == merge.Bones[k].Name && x.ParentIndex == -1); //find index of that childbone, but only if its PID is -1
                        if (minus > -1) //if we can find it AKA its PID is -1
                        {

                            int another = full.FindIndex(x => x.Name == merge.Bones[original].Name); //get index from MASTER where the bone is the same as
                            while (another < full.Count && another > 0 && full[another].ParentIndex < 0)
                            {
                                int bone = merge.Bones.FindIndex(x => x.Name == full[another].Name);
                                another = full.FindIndex(x => x.Name == merge.Bones[bone].Name);
                            }
                            full[minus].ParentIndex = (short)another;
                        }
                    }
                    else full.Add(merge.Bones[k]); //if childbone isnt in MASTER, add it
                }
            }
        }
        public void armorset(string erdir, string armor, string lefthand, string righthand, string beards, string eyebrows, string hairs, bool textures, string male = "m", bool addBodyUnder = false)
        {
            //FG101-152: faces
            //FG15XX: eyes
            //FG20XX: eyebrows
            //FG30XX: beards
            //FG50XX: eyepatch etc
            //FG70XX: eyelashes
            //WP2XXX: shields

            //eyelash 7001
            //eye 1500
            //beard 3001
            //eyebrow 2008 2007 2003
            List<BND4> mergeBND = new List<BND4>();
            List<FLVER2> merge = new List<FLVER2>(); //.Files.Where(i => Path.GetExtension(i.Name) == ".flver").ToList()[0].Bytes)
            List<bool> weights = new List<bool>();

            BND4 matbnd;
            matbnd = BND4.Read(erdir + "//material//allmaterial.matbinbnd");
            string matPathFirst = "N:\\GR\\data\\INTERROOT_win64\\material\\matbin";

            string outName = armor + "_" + lefthand + "_" + righthand + "_" + beards + "_" + "_" + hairs + "_" + eyebrows;

            mergeBND.Add(BND4.Read(erdir + $"\\chr\\c0000.chrbnd"));
            //mergeBND.Add(BND4.Read(erdir + $"\\parts\\fc_{male}_0100.partsbnd"));

            string[] partsPrefix = new string[] { "am_", "bd_", "hd_", "lg_" };
            foreach (string prefix in partsPrefix)
            {
                if (File.Exists(erdir + $"\\parts\\{prefix}m_{armor}.partsbnd"))
                {
                    mergeBND.Add(BND4.Read(erdir + $"\\parts\\{prefix}m_{armor}.partsbnd"));
                }
            }

            if (addBodyUnder)
            {
                mergeBND.Add(BND4.Read(erdir + $"\\parts\\fg_a_7001.partsbnd"));
                mergeBND.Add(BND4.Read(erdir + $"\\parts\\fg_a_1500.partsbnd"));
                mergeBND.Add(BND4.Read(erdir + $"\\parts\\fg_a_0152.partsbnd"));

                mergeBND.Add(BND4.Read(erdir + "\\parts\\fg_a_" + beards + ".partsbnd"));
                mergeBND.Add(BND4.Read(erdir + "\\parts\\fg_a_" + eyebrows + ".partsbnd"));
                mergeBND.Add(BND4.Read(erdir + "\\parts\\hr_a_" + hairs + ".partsbnd"));
            }

            List<Matrix4x4> transforms = new List<Matrix4x4>();
            Dictionary<int, List<MATBIN>> materials = new Dictionary<int, List<MATBIN>>();

            for (int i = 0; i < mergeBND.Count; i++)
            {
                merge.Add(FLVER2.Read(mergeBND[i].Files.Where(j => Path.GetExtension(j.Name) == ".flver").ToList()[0].Bytes));
                model.Add(FLVER2.Read(mergeBND[i].Files.Where(j => Path.GetExtension(j.Name) == ".flver").ToList()[0].Bytes));

                if (textures)
                {
                    if (!mergeBND[i].Files.Where(j => Path.GetExtension(j.Name) == ".tpf").ToList().Any())
                    {
                        texThreads.Add(new Thread(() => texture(TPF.Read(mergeBND[i].Files.Where(j => Path.GetExtension(j.Name) == ".tpf").ToList()[0].Bytes), Path.Combine(erdir, outName + "_tex"))));
                        texThreads.Last().Start();
                    }
                    
                    
                }

                weights.Add(false);
                transforms.Add(Matrix4x4.Identity);
            }
            mergeBND.Clear();
            List<FLVER.Bone> full = new List<FLVER.Bone>(merge[0].Bones);
            for (int i = 1; i < merge.Count; i++)
            {
                for (int j = 0; j < merge[i].Bones.Count; j++)
                {
                    if (merge[i].Bones[j].ParentIndex == -1)
                    {
                        if (!full.Any(x => x.Name == merge[i].Bones[j].Name))
                        {
                            full.Add(merge[i].Bones[j]);
                        }
                        continue;
                    }
                    if (full.Any(x => x.Name == merge[i].Bones[j].Name)) //if MASTER rig already has the bone by name
                    {
                        assembleMasterRig(full,merge[i], j);
                    }
                }
                
            }
            mergeBND.Clear();

            int Rwos = -1;
            int Lwos = -1;
            int rhand = -1;
            int lhand = -1;
            int.TryParse(righthand, out Rwos);
            int.TryParse(lefthand, out Lwos);
            if (Rwos != -1)
            {
                if (Rwos >= 2000)
                {
                    rhand = full.FindIndex(x => x.Name == "R_Shield");
                }
                else
                {
                    rhand = full.FindIndex(x => x.Name == "R_Weapon");
                }
            }
            if (Lwos != -1)
            {
                if (Lwos >= 2000)
                {
                    lhand = full.FindIndex(x => x.Name == "L_Shield");
                }
                else
                {
                    lhand = full.FindIndex(x => x.Name == "L_Weapon");
                }

            }

            threads.Add(new Thread(() => boneTrans(full,transforms,lhand,rhand)));
            threads.Last().Start();

            mergeBND.Add(BND4.Read(erdir + "\\parts\\wp_a_" + righthand + ".partsbnd"));
            mergeBND.Add(BND4.Read(erdir + "\\parts\\wp_a_" + lefthand + ".partsbnd"));
            if (File.Exists(erdir + "\\parts\\wp_a_" + righthand + "_1.partsbnd"))
            {
                //mergeBND.Add(BND4.Read(erdir + "\\parts\\wp_a_" + righthand + "_1.partsbnd"));
            }
            if (File.Exists(erdir + "\\parts\\wp_a_" + lefthand + "_1.partsbnd"))
            {
                //mergeBND.Add(BND4.Read(erdir + "\\parts\\wp_a_" + lefthand + "_1.partsbnd"));
            }
            
            merge.Clear();
            for (int i = 0; i < mergeBND.Count; i++)
            {
                merge.Add(FLVER2.Read(mergeBND[i].Files.Where(j => Path.GetExtension(j.Name) == ".flver").ToList()[0].Bytes));
                model.Add(FLVER2.Read(mergeBND[i].Files.Where(j => Path.GetExtension(j.Name) == ".flver").ToList()[0].Bytes));

                if (textures)
                {
                    if (!mergeBND[i].Files.Where(j => Path.GetExtension(j.Name) == ".tpf").ToList().Any())
                    {
                        //texThreads.Add(new Thread(() => texture(TPF.Read(mergeBND[i].Files.Where(j => Path.GetExtension(j.Name) == ".tpf").ToList()[0].Bytes), Path.Combine(erdir, outName+"_tex"))));
                        //texThreads.Last().Start();
                    }
                }

                weights.Add(true);
            }

            convertTextures(ref materials, ref matbnd, erdir, matPathFirst, Path.Combine(erdir, outName + "_tex"), textures);

            for (int i = 0; i < merge.Count; i++)
            {
                for (int j = 0; j < merge[i].Bones.Count; j++)
                {
                    if (i % 2 == 0) //right
                    {
                        if (merge[i].Bones[j].ParentIndex == -1)
                        {
                            merge[i].Bones[j].ParentIndex = (short)rhand;
                            //merge[i].Bones[j].Translation = full[rhand].Translation;
                            full.Add(merge[i].Bones[j]);
                            
                        }
                    }
                    else //left
                    {
                        if (merge[i].Bones[j].ParentIndex == -1)
                        {
                            merge[i].Bones[j].ParentIndex = (short)lhand;
                            //merge[i].Bones[j].Translation = full[lhand].Translation;
                            full.Add(merge[i].Bones[j]);
                        }
                    }
                    if (full.Any(x => x.Name == merge[i].Bones[j].Name))
                    {
                        assembleMasterRig(full, merge[i], j);
                    }
                }
            }
            while (threads.Any(x => x.IsAlive))
            {

            }
            WriteFLVERtoASCIIInOneCustomBones(erdir, outName, full, transforms, materials, weights, true, true);
            while (texThreads.Any(x => x.IsAlive))
            {

            }

            merge.Clear();
            full.Clear();
            Dispose();
            return;
        }
        private Matrix4x4[] boneTrans(List<FLVER.Bone> full, List<Matrix4x4> transforms, int lhand, int rhand)
        {
            Matrix4x4[] boneTrans = new Matrix4x4[full.Count];
            for (int i = 0; i < full.Count; i++)
            {
                short pIndex = full[i].ParentIndex;
                Matrix4x4 translation = Matrix4x4.Identity;
                //translation.M11 *= -1;
                if (pIndex != -1)
                {
                    translation = boneTrans[pIndex];
                }
                boneTrans[i] = full[i].ComputeLocalTransform() * translation;
            }
            transforms.Add(boneTrans[lhand]);
            transforms.Add(boneTrans[rhand]);
            return boneTrans;
        }

        private Dictionary<string, FLVER2> geometry;
        public void map(GAME game, string erdir, string map, string outFileName)
        {
            //lot of map object placement code from googleben's ERMapViewer!!! all credit goes to him!!!
            Stopwatch sw = new Stopwatch();
            sw.Start();


            List<string> filenames = new List<string>();
            MSBE msb = MSBE.Read(erdir + $"//map//mapstudio//{map}.msb");

            geometry = new Dictionary<string, FLVER2>();
            HashSet<string> pieceNames = new HashSet<string>();
            List<MSBE.Part> parts = new List<MSBE.Part>(msb.Parts.MapPieces);
            //List<Matrix4x4> transforms = new List<Matrix4x4>();
            Dictionary<int, List<Matrix4x4>> transforms = new Dictionary<int, List<Matrix4x4>>();
            Dictionary<int, List<MATBIN>> materials = new Dictionary<int, List<MATBIN>>();
            List<string> fails = new List<string>();

            BND4 matbnd;
            matbnd = BND4.Read(erdir + "//material//allmaterial.matbinbnd");
            
            string matPathFirst = "";
            List<string> geoms = new List<string>();

            parts.AddRange(msb.Parts.Objects);
            parts.AddRange(msb.Parts.Collisions);
            parts.AddRange(msb.Parts.ConnectCollisions);
            //parts.AddRange(msb.Parts.Unk1s);

            string gameCode = "";
            string extension = "";
            string bucket = "";
            switch (game)
            {
                case GAME.ELDEN_RING:
                    gameCode = "GR";
                    extension = "flver";
                    matPathFirst = "N:\\GR\\data\\INTERROOT_win64\\material\\matbin";
                    //mat = MTD.Read(BND4.Read(erdir + "//material//allmaterial.matbinbnd"));
                    break;
                case GAME.SEKIRO:
                    gameCode = "SE";
                    extension = "objbnd";
                    break;
                case GAME.BLOODBORNE:
                    gameCode = "SPRJ";
                    extension = "objbnd";
                    break;
                case GAME.DARK_SOULS:
                    gameCode = "DS";
                    extension = "objbnd";
                    break;
                default:
                    gameCode = "";
                    extension = "";
                    break;
            }
            for (int i = 0; i < parts.Count; i++)
            {
                pieceNames.Add(parts[i].ModelName);
            }
            model.Clear();
            for (int i = 0; i < msb.Models.MapPieces.Count; i++)
            {
                if (!pieceNames.Contains(msb.Models.MapPieces[i].Name))
                {
                    continue;
                }
                string firstPart = msb.Models.MapPieces[i].SibPath.Substring($@"N:\{gameCode}\data\Model\map\".Length, 12);
                bucket = firstPart.Substring(0,3);
                string secondPart = msb.Models.MapPieces[i].Name.Substring(1);
                string fullName = firstPart + "_" + secondPart;
                string fileName = $"/map/{bucket}/{firstPart}/{fullName}.mapbnd";

                BND4 bnd = BND4.Read(erdir+fileName);
                for (int j = 0; j < bnd.Files.Count; j++)
                {
                    if (bnd.Files[j].Name.Contains($"{secondPart}.{extension}"))
                    {
                        geoms.Add(bnd.Files[j].Name);
                        model.Add(FLVER2.Read(bnd.Files[j].Bytes));
                        filenames.Add(Path.GetFileNameWithoutExtension(bnd.Files[j].Name));
                        geometry.Add(msb.Models.MapPieces[i].Name, model.Last());
                    }
                }
            }
            convertTextures(ref materials, ref matbnd, erdir, matPathFirst, erdir + "\\" + outFileName + "\\" + outFileName + "_tex", true,fails);
            for (int i = 0; i < Model.Count; i++)
            {
                transforms.Add(Model[i].GetHashCode(), new List<Matrix4x4>());
                transforms[Model[i].GetHashCode()].Add(Matrix4x4.Identity);
            }
            if (bucket != "m60")
            {
                WriteFLVERtoASCIIInOne(erdir + "\\" + outFileName, outFileName + "_base", transforms, materials, true, true);
                while (texThreads.Any(x => x.IsAlive))
                {

                }
                model.Clear();
                transforms.Clear();
                geoms.Clear();
                materials.Clear();
                filenames.Clear();
                //GC.Collect();
            }
            switch (game)
            {
                case GAME.ELDEN_RING:
                    string geomFolder = Path.Combine(erdir, "asset", "aeg");
                    for (int i = 0; i < msb.Models.Objects.Count; i++)
                    {
                        if (!pieceNames.Contains(msb.Models.Objects[i].Name))
                        {
                            continue;
                        }
                        string start = msb.Models.Objects[i].Name.Substring(0, 6);
                        string fileName = $"/asset/aeg/{start}/{msb.Models.Objects[i].Name}.geombnd";
                        BND4 bnd = BND4.Read(erdir + fileName.ToLower());
                        for (int j = 0; j < bnd.Files.Count; j++)
                        {
                            try
                            {
                                if (bnd.Files[j].Name.Contains($"{msb.Models.Objects[i].Name}.flver"))
                                {
                                    geoms.Add(bnd.Files[j].Name);
                                    model.Add(FLVER2.Read(bnd.Files[j].Bytes));
                                    filenames.Add(Path.GetFileNameWithoutExtension(bnd.Files[j].Name));
                                    geometry.Add(msb.Models.Objects[i].Name, model.Last());
                                }
                            }
                            catch (Exception)
                            {
                                fails.Add("Cannot load FLVER2 " + bnd.Files[j].Name);
                                continue;
                            }

                        }
                    }
                    convertTextures(ref materials, ref matbnd, erdir, matPathFirst, erdir + "\\" + outFileName + "\\"+ outFileName + "_tex", true,fails);
                    break;
                default:
                    string objFolder = Path.Combine(erdir, "obj");
                    for (int i = 0; i < msb.Models.Objects.Count; i++)
                    {
                        if (!pieceNames.Contains(msb.Models.Objects[i].Name))
                        {
                            continue;
                        }
                        string start = msb.Models.Objects[i].Name.Substring(0, 6);
                        string fileName = $"/asset/aeg/{start}/{msb.Models.Objects[i].Name}.geombnd";
                        BND4 bnd = BND4.Read(erdir + fileName.ToLower());
                        for (int j = 0; j < bnd.Files.Count; j++)
                        {
                            try
                            {
                                if (bnd.Files[j].Name.Contains($"{msb.Models.Objects[i].Name}.flver"))
                                {
                                    model.Add(FLVER2.Read(bnd.Files[j].Bytes));
                                    geometry.Add(msb.Models.Objects[i].Name, model.Last());
                                }
                            }
                            catch (Exception)
                            {
                                fails.Add(bnd.Files[j].Name);
                                continue;
                            }

                        }
                    }
                    break;
            }

            for (int i = 0; i < parts.Count; i++)
            {
                FLVER2 geom;
                if (parts[i].ModelName == null)
                {
                    continue;
                }
                if (!geometry.TryGetValue(parts[i].ModelName, out geom))
                {
                    continue;
                }
                if (!transforms.ContainsKey(geom.GetHashCode()))
                {
                    transforms.Add(geom.GetHashCode(), new List<Matrix4x4>());
                    //continue;
                }
                transforms[geom.GetHashCode()].Add(Matrix4x4.Identity);
                transforms[geom.GetHashCode()][transforms[geom.GetHashCode()].Count-1] *= Matrix4x4.CreateScale(parts[i].Scale) * 
                    Matrix4x4.CreateRotationX(-ToRadians(parts[i].Rotation.X)) * 
                    Matrix4x4.CreateRotationY(-ToRadians(parts[i].Rotation.Y)) * 
                    Matrix4x4.CreateRotationZ(-ToRadians(parts[i].Rotation.Z)) * 
                    Matrix4x4.CreateTranslation(new Vector3(-parts[i].Position.X, parts[i].Position.Y, parts[i].Position.Z)); //parts[i].Position
            }


            int max = transforms.Max(e => e.Value.Count);

            /*List<Dictionary<int, List<Matrix4x4>>> transformSplit = new List<Dictionary<int, List<Matrix4x4>>>((max/1)+2);
            for (int i = 0; i < (max / 1) + 2; i++)
            {
                transformSplit.Add(new Dictionary<int, List<Matrix4x4>>());
            }

            for (int i = 0; i < Model.Count; i++)
            {
                //int count = (transforms[Model[i].GetHashCode()].Count / 5) + 1;
                if (transforms[Model[i].GetHashCode()].Count == 1)
                {
                    transformSplit[0].Add(Model[i].GetHashCode(), transforms[Model[i].GetHashCode()]);
                }
                else transformSplit[(transforms[Model[i].GetHashCode()].Count/1)+1].Add(Model[i].GetHashCode(), transforms[Model[i].GetHashCode()]);
            }*/
            List<Dictionary<int, List<Matrix4x4>>> transformSplit = new List<Dictionary<int, List<Matrix4x4>>>(Model.Count);
            for (int i = 0; i < Model.Count; i++)
            {
                //for (int j = 0; j < transforms[Model[i].GetHashCode()].Count; j++)
                {
                    transformSplit.Add(new Dictionary<int, List<Matrix4x4>>());
                    transformSplit[i].Add(Model[i].GetHashCode(), transforms[Model[i].GetHashCode()]);

                }
            }


            if (!Directory.Exists(erdir + "\\" + outFileName))
            {
                Directory.CreateDirectory(erdir + "\\" + outFileName);
            }

            while (texThreads.Any(x => x.IsAlive))
            {

            }
            for (int i = 0; i < transformSplit.Count; i++)
            {
                try
                {
                    if (transformSplit[i].Count > 0 && transformSplit[i].Values.Count > 0)
                    {
                        string filename;
                        if (Model[i].Materials.Count > 0)
                        {
                            filename = outFileName + "-" + filenames[i] + "-" + Path.GetFileNameWithoutExtension(Model[i].Materials?[0].MTD) + "-" + transformSplit[i].Min(e => e.Value.Count);
                        } else filename = outFileName + "-" + filenames[i] + "-" + transformSplit[i].Min(e => e.Value.Count);
                        WriteFLVERtoASCIIInOne(erdir + "\\" + outFileName, filename, transformSplit[i], materials, true, true);
                    }
                }
                catch (Exception)
                {

                    throw;
                }
                
            }

            //foreach (var item in transformSplit)
            {
                //if (item.Count > 0 && item.Values.Count > 0)
                {
                    //threads.Add(new Thread(() => WriteFLVERtoASCIIInOne(erdir + "\\" + outFileName, outFileName + item.Min(e => e.Value.Count) + "-" + item.Max(e => e.Value.Count), item, materials, true, true)));
                    //threads.Last().Start();
                    //string see = "chapel" + transformSplit[i].Min(e => e.Value.Count) + "-" + transformSplit[i].Max(e => e.Value.Count);
                    //WriteFLVERtoASCIIInOne(erdir, outFileName + transformSplit[i].Min(e => e.Value.Count)+"-"+transformSplit[i].Max(e => e.Value.Count), transformSplit[i], materials, true, true);
                    //WriteFLVERtoASCIIInOne(erdir + "\\" + outFileName, outFileName +item.Min(e => e.Value.Count), item, materials, true, true);
                }
            }

            File.WriteAllLines(erdir + "\\" + "\\" +outFileName + string.Format("{0:yy-MM-dd_HH-mm-ss-fff}", DateTime.Now) + "_fails.txt", fails);
            while (threads.Any(x => x.IsAlive) || texThreads.Any(x => x.IsAlive))
            {

            }
            sw.Stop();
            fails.Clear();
            fails.Add($"{sw.Elapsed.Minutes}m {sw.Elapsed.Seconds}s {sw.Elapsed.Milliseconds}ms");
            fails.Add($"{sw.Elapsed.Ticks} ticks");
            File.WriteAllLines(erdir + "\\" + outFileName + string.Format("{0:yy-MM-dd_HH-mm-ss-fff}", DateTime.Now) + "_PERFORMANCE_MULTI.txt", fails);


            geometry.Clear();
            pieceNames.Clear();
            transforms.Clear();
            materials.Clear();
            geoms.Clear();
            model.Clear();
            fails.Clear();
            Dispose();
            return;
        }

        List<string> textureNames = new List<string>();
        public void convertTextures(ref Dictionary<int, List<MATBIN>> materials, ref BND4 matbnd, string erdir, string matPathFirst, string outPath, bool extract = false, List<string> fails = null)
        {
            string temp = outPath;
            if (!Directory.Exists(outPath))
            {
                Directory.CreateDirectory(outPath);
            }
            for (int i = 0; i < Model.Count; i++)
            {
                if (!materials.ContainsKey(Model[i].GetHashCode()))
                {
                    materials.Add(Model[i].GetHashCode(), new List<MATBIN>());
                }
                else continue;
                for (int j = 0; j < Model[i].Materials.Count; j++)
                {
                    if (Model[i].Materials[j].MTD == "")
                    {
                        fails?.Add("MATBIN is empty");
                        materials[Model[i].GetHashCode()].Add(new MATBIN());
                        continue;
                    }
                    if (matbnd.Files.Where(x => x.Name == getTexturePaths(Model[i].Materials[j].MTD, matPathFirst)) == null)
                    {
                        fails?.Add("MATBIN doesn't exist inside materials: " + Model[i].Materials[j].MTD);
                        continue;
                    }
                    try
                    {
                        materials[Model[i].GetHashCode()].Add(MATBIN.Read(matbnd.Files.Where(x => x.Name == getTexturePaths(Model[i].Materials[j].MTD, matPathFirst)).FirstOrDefault().Bytes));
                    }
                    catch (Exception e)
                    {
                        fails?.Add("UNKNOWN ERROR: " + Model[i].Materials[j].MTD + " " + e.Message);
                        continue;
                    }
                    //materials[Model[i].GetHashCode()].Add(MATBIN.Read(matbnd.Files.Where(x => x.Name == getTexturePaths(Model[i].Materials[j].MTD, matPathFirst)).FirstOrDefault().Bytes));
                    if (extract)
                    {
                        for (int k = 0; k < materials[Model[i].GetHashCode()][j].Samplers.Count; k++)
                        {
                            if (materials[Model[i].GetHashCode()][j].Samplers[k].Path == "")
                            {
                                fails?.Add("TPF skipped cause it's empty");
                                continue;
                            }
                            string realPath = getRealTexturePath(materials[Model[i].GetHashCode()][j].Samplers[k].Path, erdir);
                            if (realPath == "")
                            {
                                fails?.Add("TPF skipped, something wrong with path: " + materials[Model[i].GetHashCode()][j].Samplers[k].Path);
                                continue;
                            }
                            if (textureNames.Contains(materials[Model[i].GetHashCode()][j].Samplers[k].Path))
                            {
                                fails?.Add("TPF skipped, already processed: " + materials[Model[i].GetHashCode()][j].Samplers[k].Path);
                                continue;
                            }
                            else textureNames.Add(materials[Model[i].GetHashCode()][j].Samplers[k].Path);
                            try
                            {
                                //texture(TPF.Read(realPath), erdir);
                                texThreads.Add(new Thread(() => texture(TPF.Read(realPath), outPath)));
                                texThreads.Last().Start();
                            }
                            catch (Exception)
                            {
                                fails?.Add("Cannot load TPF " + realPath);
                                continue;
                            }
                        }
                    }
                    
                }
                //transforms.Add(Model[i].GetHashCode(), new List<Matrix4x4>());
                //transforms[Model[i].GetHashCode()].Add(Matrix4x4.Identity);
            }
        }
        private string decideLang(LANG language)
        {
            string lang = "";
            switch (language)
            {
                case LANG.JPNJP:
                    lang = "jpnjp";
                    break;
                default:
                    lang = "engus";
                    break;
            }
            return lang;
        }
        private List<string> fmgOut(BND4 item, string erdir, string itemmenu, string language)
        {
            List<string> temp = new List<string>();
            for (int i = 0; i < item.Files.Count; i++)
            {
                if (!Directory.Exists(erdir + $"//msg//{language}//{itemmenu}"))
                {
                    Directory.CreateDirectory(erdir + $"//msg//{language}//{itemmenu}");
                }
                FMG fmg = FMG.Read(item.Files[i].Bytes);
                temp = new List<string>(fmg.Entries.Select(x => x.Text));
                File.WriteAllLines(erdir + $"//msg//{language}//{itemmenu}//{Path.GetFileNameWithoutExtension(item.Files[i].Name)}O.txt", temp);
            }
            return temp;
        }
        public List<string> msgMenu(string erdir, LANG language)
        {
            BND4 menu;
            string lang = decideLang(language);
            menu = BND4.Read(erdir + $"//msg//{lang}//menu.msgbnd.dcx");
            return fmgOut(menu, erdir, "menu", lang);
            
        }
        public List<string> msgItem(string erdir, LANG language)
        {
            BND4 item;
            string lang = decideLang(language);
            item = BND4.Read(erdir + $"//msg//{lang}//item.msgbnd.dcx");
            return fmgOut(item, erdir, "item", lang);
        }
        private static string getTexturePaths(string MTD, string firstPathPart)
        {
            string temp = MTD.Substring(23);
            string tempMat = "";
            if (Path.GetExtension(temp) == ".mtd")
            {
                string fileName = Path.GetFileNameWithoutExtension(temp);
                string[] tempA = temp.Split(new string[] { "\\" }, StringSplitOptions.None);
                temp = "";
                for (int x = 1; x < tempA.Length - 1; x++)
                {
                    temp += "\\" + tempA[x];
                }
                temp += "\\matxml\\" + fileName + ".matbin";
                tempMat = firstPathPart + temp;
            }
            else if (Path.GetExtension(temp) == ".matxml")
            {
                tempMat = firstPathPart + Regex.Replace(temp, @"(\.matxml)\b", ".matbin");
            }
            else
            {
                throw new NotImplementedException();
            }
            return tempMat;
        } 
        private static string getRealTexturePath(string SamplerPath, string erdir)
        {
            if (SamplerPath == "")
            {
                return "";
            }
            string[] tempA = SamplerPath.Split(new string[] { "\\" }, StringSplitOptions.None);
            string remove = Path.GetFileNameWithoutExtension(tempA[tempA.Length - 1].ToLower());
            string[] tempB = remove.Split('_');
            if (tempB.Length > 2)
            {
                remove = tempB[tempB.Length - 3] + "_" + tempB[tempB.Length - 2] + ".tpf";
            }
            //string test = Regex.Replace(tempA[tempA.Length - 1], @"(\.tif)\b", ".tpf").ToLower();
            //Regex.Replace(Regex.Replace(tempA[tempA.Length - 1], @"(\.tif)\b", ".tpf").ToLower(), @"(_1m)\b", "")
            string temp = "\\" + tempA[tempA.Length - 2].ToLower() + "\\" + remove;
            string path = erdir + "\\asset\\aet" + temp;
            if (path == "systex" || Regex.IsMatch(path, @"(systex)") || path.Contains("systex") || !File.Exists(path))
            {
                return "";
            }
            else return path;
        }
        public static Vector3 PosVecToXna(System.Numerics.Vector3 pos)
        {
            return new Vector3(pos.X, pos.Z, pos.Y);
        }
        public static Vector3 VecToXna(System.Numerics.Vector3 v)
        {
            return new Vector3(v.X, v.Z, v.Y);
        }
        public static float ToRadians(double angle)
        {
            return (float)((Math.PI / 180) * angle);
        }
        public void decompress(string folderPath, string outPath, bool recursive = true)
        {
            /*List<BND4> files = new List<BND4>();
            List<string> flverNames = new List<string>();
            Directory.CreateDirectory(outPath);
            if (recursive)
            {
                foreach (string item in Directory.GetFiles(folderPath, "*.dcx", SearchOption.AllDirectories))
                {
                    try
                    {
                        //files.Add(BND4.Read(item));
                        for (int i = 0; i < BND4.Read(item).Files.Count; i++)
                        {
                            if (Path.GetExtension(BND4.Read(item).Files[i].Name) == ".flver")
                            {

                                Model.Add(FLVER2.Read(BND4.Read(item).Files[i].Bytes));
                                flverNames.Add(Path.GetFileNameWithoutExtension(BND4.Read(item).Files[i].Name));

                            }

                        }

                    }
                    catch (Exception)
                    {

                        continue;
                    }


                }
                for (int i = 0; i < Model.Count; i++)
                {
                    WriteFLVERtoASCII(outPath, flverNames[i], true, false, i);
                }
                Model.Clear();
            }*/
        }
        public void Dispose()
        {
            model.Clear();
            model = null;


        }
    }
}

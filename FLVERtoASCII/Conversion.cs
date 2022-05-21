﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using SoulsFormats;


namespace FLVERtoASCII
{
    class Conversion
    {
        private List<string> ascii;
        private bool IsPlayer;
        List<FLVER2> model;
        public List<FlverBoneInfo> FlverSkeleton = new List<FlverBoneInfo>();
        public List<int> TopLevelFlverBoneIndices = new List<int>();
        public int RightWeaponBoneIndex = -1;
        public int LeftWeaponBoneIndex = -1;
        private int _headID = -1;
        private int _bodyID = -1;
        private int _armsID = -1;
        private int _legsID = -1;
        private int _rightWeaponID = -1;
        private int _leftWeaponID = -1;
        private Dictionary<string, int> boneIndexRemap = new Dictionary<string, int>();

        public List<FLVER2> Model
        {
            get { return model; }
            set { model = value; }
        }

        public Conversion(FLVER2 model, bool IsPlayer = false)
        {
            this.IsPlayer = IsPlayer;
            Model = new List<FLVER2>();
            Model.Add(model);
            ascii = new List<string>();
        }

        public Conversion(List<FLVER2> models, bool IsPlayer = false)
        {
            this.IsPlayer = IsPlayer;
            Model = new List<FLVER2>(models);
            //Model.Add(model);
            ascii = new List<string>();
        }

        public Conversion()
        {
            Model = new List<FLVER2>();
            ascii = new List<string>();
        }

        public void WriteFLVERtoASCII(string outPath, string fileName, bool bones = false, bool addRoot = false, int index = 0)
        {
            int plusBone = 0;
            if (bones)
            {

                if (addRoot)
                {
                    plusBone = 1;
                }
                var boneMatrices = new Matrix4x4[Model[index].Bones.Count + plusBone];
                ascii.Add((Model[index].Bones.Count + plusBone).ToString()); //Bone count
                List<string> boneNames = new List<string>();
                List<int> countIndex = new List<int>();
                if (addRoot)
                {
                    ascii.Add("root");
                    boneNames.Add("root");
                    ascii.Add("-1");
                    ascii.Add("0 0 0 0 0 0 1");
                }

                for (int i = 0; i < Model[index].Bones.Count; i++) //Bone names, parents, xyz
                {
                    countIndex.Add(0);
                    for (int j = 0; j < boneNames.Count; j++)
                    {
                        if (boneNames[j] == Model[index].Bones[i].Name)
                        {
                            countIndex[i]++;
                        }
                    }
                    boneNames.Add(Model[index].Bones[i].Name);
                    //ascii.Add(countIndex[i] == 0 ? Model[index].Bones[i].Name : Model[index].Bones[i].Name + countIndex[i]);
                    ascii.Add(Model[index].Bones[i].Name);
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
                    //Quaternion rot = Quaternion.CreateFromRotationMatrix(boneMatrices[i]);
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
            uint VertSum = 0;
            uint MeshSum = 0;
            for (int i = 0; i < Model[index].Meshes.Count; i++) //overall meshes
            {
                MeshSum++;
            }
            ascii.Add(MeshSum.ToString());
            int Tex = 0;
            for (int i = 0; i < Model[index].Materials.Count; i++)
            {
                Tex++;
                /*for (int j = 0; j < Model[index].Materials[i].Textures.Count; j++)
                {

                }*/
            }
            for (int i = 0; i < Model[index].Meshes.Count; i++)
            {
                for (int j = 0; j < Model[index].Meshes[i].Vertices.Count; j++) //overall vertices in one mesh
                {
                    VertSum++;
                }
                ascii.Add(Model[index].Materials[Model[index].Meshes[i].MaterialIndex].Name); //mesh name
                ascii.Add(Model[index].Meshes[i].Vertices[0].UVs.Count.ToString()); //mesh UV channel count
                ascii.Add(Tex.ToString()); //tex count
                for (int k = 0; k < Tex; k++)
                {
                    ascii.Add(Model[index].Materials[Model[index].Meshes[i].MaterialIndex].MTD);
                    ascii.Add("0");
                }
                ascii.Add(VertSum.ToString()); //mesh vertices count
                VertSum = 0;
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

            using (StreamWriter outputFile = new StreamWriter(Path.Combine(outPath, fileName + ".ascii")))
            {
                foreach (string line in ascii)
                    outputFile.WriteLine(line);
            }

            ascii.Clear();

        }
        public void WriteFLVERtoASCIIInOne(string outPath, string fileName, Dictionary<int, List<Matrix4x4>> transform, bool bones = false, bool addRoot = false)
        {

            Dictionary<int, Matrix4x4> transformOne = new Dictionary<int, Matrix4x4>();
            Dictionary<int, List<Matrix4x4>> transformMultpile = new Dictionary<int, List<Matrix4x4>>();

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

            //int prevBones = 0;
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
                        //ascii.Add(countIndex[i] == 0 ? Model[index].Bones[i].Name : Model[index].Bones[i].Name + countIndex[i]);
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
                        //Quaternion rot = Quaternion.CreateFromRotationMatrix(boneMatrices[i]);
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
                if (!transform.ContainsKey(Model[y].GetHashCode()))
                {
                    continue;
                }
                for (int z = 0; z < transform[Model[y].GetHashCode()].Count; z++)
                {
                    int Tex = 0;
                    for (int i = 0; i < Model[y].Materials.Count; i++)
                    {
                        Tex++;
                    }
                    for (int i = 0; i < Model[y].Meshes.Count; i++)
                    {
                        ascii.Add(Model[y].Materials[Model[y].Meshes[i].MaterialIndex].Name); //mesh name
                        ascii.Add(Model[y].Meshes[i].Vertices[0].UVs.Count.ToString()); //mesh UV channel count
                        ascii.Add(Tex.ToString()); //tex count
                        for (int k = 0; k < Tex; k++)
                        {
                            ascii.Add(Model[y].Materials[Model[y].Meshes[i].MaterialIndex].MTD);
                            ascii.Add("0");
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

                            /*ascii.Add(-Model[y].Meshes[i].Vertices[j].Position.X + " " +
                                      Model[y].Meshes[i].Vertices[j].Position.Y + " " +
                                      Model[y].Meshes[i].Vertices[j].Position.Z);*/
                            /*ascii.Add((transformedPos.X - Model[y].Meshes[i].Vertices[j].Position.X) + " " +
                                      (transformedPos.Y + Model[y].Meshes[i].Vertices[j].Position.Y) + " " +
                                      (transformedPos.Z + Model[y].Meshes[i].Vertices[j].Position.Z));*/
                            ascii.Add(transformedPos.X + " " +
                                      transformedPos.Y + " " +
                                      transformedPos.Z);
                            //vert norm

                            /*ascii.Add(-Model[e].Meshes[i].Vertices[j].Normal.X + " " +
                                      Model[e].Meshes[i].Vertices[j].Normal.Y + " " +
                                      Model[e].Meshes[i].Vertices[j].Normal.Z);*/

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

                }
            }

            using (StreamWriter outputFile = new StreamWriter(Path.Combine(outPath, fileName + ".ascii")))
            {
                foreach (string line in ascii)
                    outputFile.WriteLine(line);
            }

            ascii.Clear();

        }

        public void chrbndFolder(string inPath, string outPath)
        {
            string[] tomb = Directory.GetFiles(inPath, "*.chrbnd");
            string[] tombP = Directory.GetFiles(inPath, "*.partsbnd");
            string[] texs = Directory.GetFiles(inPath, "*.texbnd");
            List<string> filenames = new List<string>();
            List<string> filenamesP = new List<string>();
            List<string> filenamesTex = new List<string>();
            List<BND4> chrbnds = new List<BND4>();
            //List<BND4> partbnds = new List<BND4>();
            List<BND4> texbnds = new List<BND4>();
            for (int i = 0; i < tombP.Length; i++)
            {
                BND4 partbnd = BND4.Read(tombP[i]);
                //partbnds.Add(BND4.Read(tombP[i]));
                for (int j = 0; j < partbnd.Files.Count; j++)
                {
                    if (Path.GetExtension(partbnd.Files[j].Name).ToLower() == ".flver")
                    {
                        Model.Add(FLVER2.Read(partbnd.Files[j].Bytes));
                        filenamesP.Add(Path.GetFileNameWithoutExtension(partbnd.Files[j].Name));
                    }
                    /*else if (Path.GetExtension(partbnds[i].Files[j].Name).ToLower() == ".tpf")
                    {
                        //File.WriteAllBytes(outPath+"//"+Path.GetFileName(chrbnds[i].Files[j].Name).ToLower(), chrbnds[i].Files[j].Bytes);
                        Directory.CreateDirectory(outPath + "//" + Path.GetFileName(partbnds[i].Files[j].Name));
                        texture(TPF.Read(partbnds[i].Files[j].Bytes), outPath + "//" + Path.GetFileName(partbnds[i].Files[j].Name));
                    }*/

                    //chrbnds[i].Files[j].Name;
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
                    else if (Path.GetExtension(chrbnds[i].Files[j].Name).ToLower() == ".tpf")
                    {
                        //File.WriteAllBytes(outPath+"//"+Path.GetFileName(chrbnds[i].Files[j].Name).ToLower(), chrbnds[i].Files[j].Bytes);
                        Directory.CreateDirectory(outPath + "//" + Path.GetFileName(chrbnds[i].Files[j].Name));
                        texture(TPF.Read(chrbnds[i].Files[j].Bytes), outPath + "//" + Path.GetFileName(chrbnds[i].Files[j].Name));
                    }

                    //chrbnds[i].Files[j].Name;
                }


            }
            /*for (int i = 0; i < texs.Length; i++)
            {
                texbnds.Add(BND4.Read(texs[i]));
                for (int j = 0; j < texbnds[i].Files.Count; j++)
                {
                    if (Path.GetExtension(texbnds[i].Files[j].Name).ToLower() == ".tpf")
                    {
                        //File.WriteAllBytes(outPath+"//"+Path.GetFileName(chrbnds[i].Files[j].Name).ToLower(), chrbnds[i].Files[j].Bytes);
                        Directory.CreateDirectory(outPath + "//" + Path.GetFileName(texbnds[i].Files[j].Name));
                        texture(TPF.Read(texbnds[i].Files[j].Bytes), outPath + "//" + Path.GetFileName(texbnds[i].Files[j].Name));
                    }
                }
            }*/
            for (int i = 0; i < Model.Count; i++)
            {
                WriteFLVERtoASCII(outPath, filenames[i], true, true, i);
            }
        }

        public void texture(TPF text, string outPath)
        {
            for (int i = 0; i < text.Textures.Count; i++)
            {
                File.WriteAllBytes(Path.Combine(outPath, text.Textures[i].Name + ".dds"), text.Textures[i].Bytes);
            }

        }

        public void armorset(string erdir, string armor, string lefthand = "", string righthand = "")
        {
            FLVER2 body = FLVER2.Read(BND4.Read(erdir + "\\parts\\bd_m_" + armor + ".partsbnd").Files.Where(i => Path.GetExtension(i.Name) == ".flver").ToList()[0].Bytes);
            FLVER2 head = FLVER2.Read(BND4.Read(erdir + "\\parts\\hd_m_" + armor + ".partsbnd").Files.Where(i => Path.GetExtension(i.Name) == ".flver").ToList()[0].Bytes);
            FLVER2 arms = FLVER2.Read(BND4.Read(erdir + "\\parts\\am_m_" + armor + ".partsbnd").Files.Where(i => Path.GetExtension(i.Name) == ".flver").ToList()[0].Bytes);
            FLVER2 legs = FLVER2.Read(BND4.Read(erdir + "\\parts\\lg_m_" + armor + ".partsbnd").Files.Where(i => Path.GetExtension(i.Name) == ".flver").ToList()[0].Bytes);

            List<FLVER.Bone> overallBones = new List<FLVER.Bone>(body.Bones);
            for (int i = 0; i < head.Bones.Count; i++)
            {
                for (int j = 0; j < overallBones.Count; j++)
                {
                    if (head.Bones[i].Name != overallBones[j].Name)
                    {
                        overallBones.Add(head.Bones[i]);
                    }
                }

            }
            /*
            int plusBone = 1;
            var boneMatrices = new Matrix4x4[Model[index].Bones.Count + plusBone];
            ascii.Add((Model[index].Bones.Count + plusBone).ToString()); //Bone count
            List<string> boneNames = new List<string>();
            List<int> countIndex = new List<int>();
            if (addRoot)
            {
                ascii.Add("root");
                boneNames.Add("root");
                ascii.Add("-1");
                ascii.Add("0 0 0 0 0 0 1");
            }

            for (int i = 0; i < Model[index].Bones.Count; i++) //Bone names, parents, xyz
            {
                countIndex.Add(0);
                for (int j = 0; j < boneNames.Count; j++)
                {
                    if (boneNames[j] == Model[index].Bones[i].Name)
                    {
                        countIndex[i]++;
                    }
                }
                boneNames.Add(Model[index].Bones[i].Name);
                //ascii.Add(countIndex[i] == 0 ? Model[index].Bones[i].Name : Model[index].Bones[i].Name + countIndex[i]);
                ascii.Add(Model[index].Bones[i].Name);
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
                //Quaternion rot = Quaternion.CreateFromRotationMatrix(boneMatrices[i]);
                ascii.Add((pIndex + plusBone).ToString());
                ascii.Add(boneMatrices[i].M41.ToString("0.##########") + " " +
                          boneMatrices[i].M42.ToString("0.##########") + " " +
                          boneMatrices[i].M43.ToString("0.##########"));
            }


            uint VertSum = 0;
            uint MeshSum = 0;
            for (int i = 0; i < Model[index].Meshes.Count; i++) //overall meshes
            {
                MeshSum++;
            }
            ascii.Add(MeshSum.ToString());
            int Tex = 0;
            for (int i = 0; i < Model[index].Materials.Count; i++)
            {
                Tex++;
            }
            for (int i = 0; i < Model[index].Meshes.Count; i++)
            {
                for (int j = 0; j < Model[index].Meshes[i].Vertices.Count; j++) //overall vertices in one mesh
                {
                    VertSum++;
                }
                ascii.Add(Model[index].Materials[Model[index].Meshes[i].MaterialIndex].Name); //mesh name
                ascii.Add(Model[index].Meshes[i].Vertices[0].UVs.Count.ToString()); //mesh UV channel count
                ascii.Add(Tex.ToString()); //tex count
                for (int k = 0; k < Tex; k++)
                {
                    ascii.Add(Model[index].Materials[Model[index].Meshes[i].MaterialIndex].MTD);
                    ascii.Add("0");
                }
                ascii.Add(VertSum.ToString()); //mesh vertices count
                VertSum = 0;
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

            using (StreamWriter outputFile = new StreamWriter(Path.Combine(outPath, fileName + ".ascii")))
            {
                foreach (string line in ascii)
                    outputFile.WriteLine(line);
            }

            ascii.Clear();*/
        }
        public void MergePlayer(string outPath, string fileName, bool bones = false, bool addRoot = false)
        {
            int plusBone = 0;
            if (addRoot)
            {
                plusBone = 1;
            }

            boneIndexRemap = new Dictionary<string, int>();
            List<int> boundary = new List<int>();
            List<string> boneNames = new List<string>();
            int boneIndex = 0;
            for (int i = 0; i < model.Count; i++)
            {
                for (int j = 0; j < model[i].Bones.Count; j++)
                {
                    if (!boneIndexRemap.ContainsKey(model[i].Bones[j].Name))
                    {
                        boneIndexRemap.Add(model[i].Bones[j].Name, boneIndex);
                        boneNames.Add(model[i].Bones[j].Name);
                        boneIndex++;
                    }
                }
                boundary.Add(boneIndex);
            }

            var boneMatrices = new Matrix4x4[boneIndexRemap.Count + plusBone];
            ascii.Add((boneIndexRemap.Count + plusBone).ToString()); //Bone count

            List<int> countIndex = new List<int>();
            if (addRoot)
            {
                ascii.Add("root");
                boneNames.Add("root");
                ascii.Add("-1");
                ascii.Add("0 0 0 0 0 0 1");
            }

            //short pIndex = Model[index].Bones[i].ParentIndex;
            for (int i = 0; i < boneIndexRemap.Count; i++)
            {
                ascii.Add(boneIndexRemap.Keys.ToList()[i]);
                //pIndex
            }




            /*for (int i = 0; i < Model[index].Bones.Count; i++) //Bone names, parents, xyz
            {
                countIndex.Add(0);
                for (int j = 0; j < boneNames.Count; j++)
                {
                    if (boneNames[j] == Model[index].Bones[i].Name)
                    {
                        countIndex[i]++;
                    }
                }
                //boneNames.Add(Model[index].Bones[i].Name);
                //ascii.Add(countIndex[i] == 0 ? Model[index].Bones[i].Name : Model[index].Bones[i].Name + countIndex[i]);
                
                
                Matrix4x4 translation = Matrix4x4.Identity;

                if (pIndex != -1)
                {
                    translation = boneMatrices[pIndex];
                }
                boneMatrices[i] = Model[index].Bones[i].ComputeLocalTransform() * translation;
                //Quaternion rot = Quaternion.CreateFromRotationMatrix(boneMatrices[i]);
                ascii.Add((pIndex + plusBone).ToString());
                ascii.Add(boneMatrices[i].M41.ToString("0.##########") + " " +
                          boneMatrices[i].M42.ToString("0.##########") + " " +
                          boneMatrices[i].M43.ToString("0.##########"));

            }*/

            for (int index = 0; index < model.Count; index++)
            {

                uint VertSum = 0;
                uint MeshSum = 0;
                for (int i = 0; i < Model[index].Meshes.Count; i++) //overall meshes
                {
                    MeshSum++;
                }
                ascii.Add(MeshSum.ToString());
                int Tex = 0;
                for (int i = 0; i < Model[index].Materials.Count; i++)
                {
                    Tex++;
                    /*for (int j = 0; j < Model[index].Materials[i].Textures.Count; j++)
                    {

                    }*/
                }
                for (int i = 0; i < Model[index].Meshes.Count; i++)
                {
                    for (int j = 0; j < Model[index].Meshes[i].Vertices.Count; j++) //overall vertices in one mesh
                    {
                        VertSum++;
                    }
                    ascii.Add(Model[index].Materials[Model[index].Meshes[i].MaterialIndex].Name); //mesh name
                    ascii.Add(Model[index].Meshes[i].Vertices[0].UVs.Count.ToString()); //mesh UV channel count
                    ascii.Add(Tex.ToString()); //tex count
                    for (int k = 0; k < Tex; k++)
                    {
                        ascii.Add(Model[index].Materials[Model[index].Meshes[i].MaterialIndex].MTD);
                        ascii.Add("0");
                    }
                    ascii.Add(VertSum.ToString()); //mesh vertices count
                    VertSum = 0;
                    for (int j = 0; j < Model[index].Meshes[i].Vertices.Count; j++) //vertices
                    {
                        //vert pos
                        ascii.Add(Model[index].Meshes[i].Vertices[j].Position.X + " " +
                                  Model[index].Meshes[i].Vertices[j].Position.Y + " " +
                                  Model[index].Meshes[i].Vertices[j].Position.Z);
                        //vert norm
                        ascii.Add(Model[index].Meshes[i].Vertices[j].Normal.X + " " +
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
                                  Model[index].Meshes[i].FaceSets[0].Indices[j + 1] + " " +
                                  Model[index].Meshes[i].FaceSets[0].Indices[j + 2]);
                        j++;
                        j++;
                    }

                }
            }




            using (StreamWriter outputFile = new StreamWriter(Path.Combine(outPath, fileName + ".ascii")))
            {
                foreach (string line in ascii)
                    outputFile.WriteLine(line);
            }

            ascii.Clear();

        }

        public Dictionary<string, FLVER2> geometry;
        public void map(Games game, string erdir, MSBE msb, string outFileName)
        {
            geometry = new Dictionary<string, FLVER2>();
            HashSet<string> pieceNames = new HashSet<string>();
            List<MSBE.Part> parts = new List<MSBE.Part>(msb.Parts.MapPieces);
            //List<Matrix4x4> transforms = new List<Matrix4x4>();
            Dictionary<int, List<Matrix4x4>> transforms = new Dictionary<int, List<Matrix4x4>>();

            List<string> fails = new List<string>();

            List<Vector3> translations = new List<Vector3>();
            List<Vector3> rotations = new List<Vector3>();
            List<Vector3> scale = new List<Vector3>();

            parts.AddRange(msb.Parts.Objects);
            parts.AddRange(msb.Parts.Collisions);
            parts.AddRange(msb.Parts.ConnectCollisions);
            //parts.AddRange(msb.Parts.Unk1s);
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
                string firstPart = msb.Models.MapPieces[i].SibPath.Substring(@"N:\GR\data\Model\map\".Length, 12);
                string bucket = firstPart.Substring(0,3);
                string secondPart = msb.Models.MapPieces[i].Name.Substring(1);
                string fullName = firstPart + "_" + secondPart;
                string fileName = $"/map/{bucket}/{firstPart}/{fullName}.mapbnd";

                BND4 bnd = BND4.Read(erdir+fileName);
                for (int j = 0; j < bnd.Files.Count; j++)
                {
                    if (bnd.Files[j].Name.Contains($"{secondPart}.flver"))
                    {
                        model.Add(FLVER2.Read(bnd.Files[j].Bytes));
                        geometry.Add(msb.Models.MapPieces[i].Name, model.Last());
                        //FLVER2 flver = FLVER2.Read(bnd.Files[j].Bytes);
                    }
                }
            }
            for (int i = 0; i < Model.Count; i++)
            {
                transforms.Add(Model[i].GetHashCode(), new List<Matrix4x4>());
                transforms[Model[i].GetHashCode()].Add(Matrix4x4.Identity);
            }
            WriteFLVERtoASCIIInOne(erdir, outFileName+"_base", transforms, true, true);
            model.Clear();
            transforms.Clear();

            switch (game)
            {
                case Games.ELDEN_RING:

                    break;
                default:
                    break;
            }

            string geomFolder = Path.Combine(erdir, "asset", "aeg");
            for (int i = 0; i < msb.Models.Objects.Count; i++)
            {
                if (!pieceNames.Contains(msb.Models.Objects[i].Name))
                {
                    continue;
                }
                string start = msb.Models.Objects[i].Name.Substring(0,6);
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

            List<Dictionary<int, List<Matrix4x4>>> transformSplit = new List<Dictionary<int, List<Matrix4x4>>>((max/1)+2);
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
            }
            
            for (int i = 0; i < transformSplit.Count; i++)
            {
                if (transformSplit[i].Count > 0)
                {
                    //string see = "chapel" + transformSplit[i].Min(e => e.Value.Count) + "-" + transformSplit[i].Max(e => e.Value.Count);
                    WriteFLVERtoASCIIInOne(erdir, outFileName + transformSplit[i].Min(e => e.Value.Count)+"-"+transformSplit[i].Max(e => e.Value.Count), transformSplit[i], true, true);
                }
            }
            ;
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
        private void writeBones()
        {

        }
        private void writeSumsOne()
        {

        }
    }
}
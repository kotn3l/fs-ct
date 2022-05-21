using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoulsFormats;
using System.Numerics;

namespace FLVERtoASCII
{
    class Skeleton
    {
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

        public Skeleton(FLVER2 flvr)
        {
            int[] childCounts = new int[flvr.Bones.Count];

            FlverSkeleton.Clear();
            TopLevelFlverBoneIndices.Clear();

            for (int i = 0; i < flvr.Bones.Count; i++)
            {
                if (flvr.Bones[i].ParentIndex < 0)
                    TopLevelFlverBoneIndices.Add(i);
                var newBone = new FlverBoneInfo(flvr.Bones[i], flvr.Bones);
                if (flvr.Bones[i].ParentIndex >= 0)
                    childCounts[flvr.Bones[i].ParentIndex]++;
                FlverSkeleton.Add(newBone);
            }
            boneIndexRemap = new Dictionary<string, int>();
            for (int i = 0; i < FlverSkeleton.Count; i++)
            {
                if (FlverSkeleton[i].Name == "R_Weapon")
                {
                    RightWeaponBoneIndex = i;
                }
                else if (FlverSkeleton[i].Name == "L_Weapon")
                {
                    LeftWeaponBoneIndex = i;
                }
                if (!boneIndexRemap.ContainsKey(FlverSkeleton[i].Name))
                {
                    boneIndexRemap.Add(FlverSkeleton[i].Name, i);
                }
                if (flvr.Bones[i].ParentIndex >= 0 && flvr.Bones[i].ParentIndex < flvr.Bones.Count)
                {
                    FlverSkeleton[flvr.Bones[i].ParentIndex].ChildBones.Add(FlverSkeleton[i]);
                }
            }

            

        }

        public void LoadArmor(FLVER2 flvr)
        {
            Dictionary<int, int> finalBoneRemapper = null;
            if (boneIndexRemap != null)
            {
                finalBoneRemapper = new Dictionary<int, int>();
                for (int i = 0; i < flvr.Bones.Count; i++)
                {
                    if (boneIndexRemap.ContainsKey(flvr.Bones[i].Name))
                    {
                        finalBoneRemapper.Add(i, boneIndexRemap[flvr.Bones[i].Name]);
                    }
                    else if (boneIndexRemap.ContainsKey(flvr.Bones[0].Name))
                    {
                        finalBoneRemapper.Add(i, boneIndexRemap[flvr.Bones[0].Name]);
                    }
                }
            }
        }


    }
    public class FlverBoneInfo
    {
        public string Name;
        public int ParentIndex;
        public Matrix4x4 ParentReferenceMatrix = Matrix4x4.Identity;
        public Matrix4x4 ReferenceMatrix = Matrix4x4.Identity;
        public int HkxBoneIndex = -1;
        public Matrix4x4 CurrentMatrix = Matrix4x4.Identity;

        public Matrix4x4? NubReferenceMatrix = null;

        public List<FlverBoneInfo> ChildBones = new List<FlverBoneInfo>();
        public FlverBoneInfo(FLVER.Bone bone, List<FLVER.Bone> boneList)
        {
            ParentIndex = bone.ParentIndex;

            Matrix4x4 GetBoneMatrix(SoulsFormats.FLVER.Bone b, bool saveParentBone)
            {
                SoulsFormats.FLVER.Bone parentBone = b;

                var result = Matrix4x4.Identity;

                bool isTopBone = true;

                do
                {
                    result *= Matrix4x4.CreateScale(parentBone.Scale.X, parentBone.Scale.Y, parentBone.Scale.Z);
                    result *= Matrix4x4.CreateRotationX(parentBone.Rotation.X);
                    result *= Matrix4x4.CreateRotationZ(parentBone.Rotation.Z);
                    result *= Matrix4x4.CreateRotationY(parentBone.Rotation.Y);
                    result *= Matrix4x4.CreateTranslation(parentBone.Translation.X, parentBone.Translation.Y, parentBone.Translation.Z);

                    if (parentBone.ParentIndex >= 0)
                        parentBone = boneList[parentBone.ParentIndex];
                    else
                        parentBone = null;

                    isTopBone = false;

                    if (saveParentBone && isTopBone)
                        ParentReferenceMatrix = GetBoneMatrix(parentBone, saveParentBone: false);
                }
                while (parentBone != null);

                return result;
            }

            ReferenceMatrix = GetBoneMatrix(bone, saveParentBone: true);
            Name = bone.Name;

            if (bone.Unk3C == 0)
            {
                var nubBone = boneList.Where(bn => bn.Name == bone.Name + "Nub").FirstOrDefault();

                if (nubBone != null)
                {
                    var nubMat = Matrix4x4.Identity;
                    nubMat *= Matrix4x4.CreateScale(nubBone.Scale.X, nubBone.Scale.Y, nubBone.Scale.Z);
                    nubMat *= Matrix4x4.CreateRotationX(nubBone.Rotation.X);
                    nubMat *= Matrix4x4.CreateRotationZ(nubBone.Rotation.Z);
                    nubMat *= Matrix4x4.CreateRotationY(nubBone.Rotation.Y);
                    nubMat *= Matrix4x4.CreateTranslation(nubBone.Translation.X, nubBone.Translation.Y, nubBone.Translation.Z);

                    NubReferenceMatrix = nubMat;
                }


            }

        }
    }
}

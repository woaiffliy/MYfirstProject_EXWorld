﻿using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace  Managers
{
    class MinimapManager : Singleton<MinimapManager>
    {

        private Collider minimapBoundingBox;
        public Collider MinimapBoundingBox
        {
            get { return minimapBoundingBox; }
        }

        public UIMinimap minimap;


        public Transform PlayerTransform
        { 
            get
            {
                if (User.Instance.CurrentCharacterObject == null)
                    return null;
                return User.Instance.CurrentCharacterObject.transform;
            }
                
                
                }

        public Sprite LoadCurrentMinimap()
        {
            return Resloader.Load<Sprite>("UI/Minimap/" + User.Instance.CurrentMap.MiniMap);
        }

        public void UpdateMinimap(Collider minimapBoundingBox)
        {
            this.minimapBoundingBox = minimapBoundingBox;
            if (minimap != null)
            {
                this.minimap.UpdateMap();
            }
        }
    }
}

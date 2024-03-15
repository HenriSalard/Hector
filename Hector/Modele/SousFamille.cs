﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hector.Modele
{
    class SousFamille
    {
        private int refSousFamille;

        private int refFamille;

        private string nomSousFamille;

        public SousFamille(int refSousFamille, int refFamille, string nomSousFamille)
        {
            this.RefSousFamille = refSousFamille;
            this.RefFamille = refFamille;
            this.NomSousFamille = nomSousFamille;
        }

        public int RefSousFamille { get => refSousFamille; set => refSousFamille = value; }

        public int RefFamille { get => refFamille; set => refFamille = value; }

        public string NomSousFamille { get => nomSousFamille; set => nomSousFamille = value; }
    }
}
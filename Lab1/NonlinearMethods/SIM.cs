using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab1;
using Vectors;

namespace NonlinearMethods {
    class SIM {
        Vector NMsolution;
        Vector startVector;
        Vector curVector;
        Vector prevVector;
        Functions f = new Functions();
        public SIM(Vector solution, Vector startVector) {
            this.NMsolution = solution;
            this.startVector = startVector;
            prevVector = startVector;
            SimpleIterationMethod();
        }

        public void SimpleIterationMethod() {
            int itr = 1;
            do {
                curVector = f.Fi(prevVector);
                prevVector = curVector;

            }
            while((f.F(curVector) - f.F(NMsolution)).Norm() >= 1E-4);
        }

        //Console.Write(itr++ + " ");
        //Console.Write(curVector.ToString() + " ");
        //Console.Write((f.F(curVector) - f.F(NMsolution)).Norm() + " ");
        //Console.WriteLine((curVector - NMsolution).Norm());
    }
}

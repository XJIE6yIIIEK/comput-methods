using System;
using Matrices;
using Vectors;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NonlinearMethods {
    class Functions{
        public double dF1dx(Vector X) {
            return 1.0;
        }

        public double dF1dy(Vector X) {
            return -Math.Sin(X[1] - 1);
        }

        public double dF2dx(Vector X)  {
            return Math.Sin(X[0]);
        }

        public double dF2dy(Vector X) {
            return 1.0;
        }

        public Vector F(Vector X) {
            Vector v = new Vector(2);
            v[0] = F1(X);
            v[1] = F2(X);
            return v;
        }
        public double F1(Vector X) {
            return Math.Cos(X[1] - 1) + X[0] - 0.8;
        }

        public double F2(Vector X) {
            return X[1] - Math.Cos(X[0]) - 2;
        }

        public Matrix derivF(Vector X) {
            Matrix a = new Matrix(2, 2);
            a[0, 0] = dF1dx(X);
            a[0, 1] = dF1dy(X);
            a[1, 0] = dF2dx(X);
            a[1, 1] = dF2dy(X);
            return a.Inverse;
        }

        public double Fi1(Vector X) {
            return 0.8 - Math.Cos(X[1] - 1);
        }

        public double Fi2(Vector X)
        {
            return 2 + Math.Cos(X[0]);
        }

        public Vector Fi(Vector X){
            Vector v = new Vector(2);
            v[0] = Fi1(X);
            v[1] = Fi2(X);
            return v;
        }
    }
}

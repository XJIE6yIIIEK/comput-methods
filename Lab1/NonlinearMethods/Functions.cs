using System;
using Matrices;
using Vectors;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NonlinearMethods {
    interface _Functions {
        double EvaluateDerivativeX(Vector X);

        double EvaluateDerivativeY(Vector X);

        double Evaluate(Vector X);
    }

    class F1 : _Functions {
        public double EvaluateDerivativeX(Vector X) {
            return 1.0;
        }

        public double EvaluateDerivativeY(Vector X) {
            return -Math.Sin(X[1] - 1);
        }

        public double Evaluate(Vector X) {
            return Math.Cos(X[1] - 1) + X[0] - 0.8;
        }
    }

    class F2 : _Functions {
        public double EvaluateDerivativeX(Vector X) {
            return Math.Sin(X[0]);
        }

        public double EvaluateDerivativeY(Vector X) {
            return 1.0;
        }

        public double Evaluate(Vector X) {
            return X[1] - Math.Cos(X[0]) - 2;
        }
    }

    class Fi1 : _Functions {
        public double EvaluateDerivativeX(Vector X) {
            return 0.0;
        }

        public double EvaluateDerivativeY(Vector X) {
            return Math.Sin(X[1] - 1);
        }

        public double Evaluate(Vector X) {
            return 0.8 - Math.Cos(X[1] - 1);
        }
    }

    class Fi2 : _Functions {
        public double EvaluateDerivativeX(Vector X) {
            return -Math.Sin(X[0]);
        }

        public double EvaluateDerivativeY(Vector X) {
            return 0.0;
        }

        public double Evaluate(Vector X) {
            return 2 + Math.Cos(X[0]);
        }
    }
}
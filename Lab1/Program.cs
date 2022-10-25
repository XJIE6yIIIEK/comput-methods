using System;
using Matrices;
using Vectors;
using Methods;
using NonlinearMethods;
using System.Runtime.Serialization;

namespace Lab1 {
	enum Tasks { 
		StraightMethods = 1,
		IterativeMethods = 2,
        NonlinearMethods = 3
	}

    class Program {

        static IOModule io;

        static void StraightMethods()
        {
            io.ExcelInit();

            int n;
            io.GetSize(out n);

            Matrix A = new Matrix(n, n);
            Vector X = new Vector(n);

            io.GetMatrixInfo(A, "A");
            io.GetVectorInfo(X, "X");

            Vector B = A * X;

            io.TerminateExcel();
            io.FileOpen();

            io.WriteLine("INPUT");
            io.WriteLine("X=");
            io.WriteLine(X.ToString(), 1);
            io.WriteLine("A=");
            io.WriteLine(A.ToString(), 1);
            io.WriteLine("B=");
            io.WriteLine(B.ToString(), 1);
            io.SeparateText();

            LU lu = new LU(A, B, n, io);

            Vector _X = lu.X;
            Matrix X_diff = (lu.L * lu.U) - lu.A;

            io.WriteLine("X_Eval =");
            io.WriteLine(_X.ToString(), 1);
            io.WriteLine("L * U - P * A =");
            io.WriteLine(X_diff.ToString(), 1);
            io.SeparateText();

            //Нахождение обратной матрицы
            Matrix A_INVERSE = lu.U.Inverse * lu.L.Inverse * lu.E.FastSwap(lu.m);
            //Проверка
            Matrix A_diff = lu.A * A_INVERSE - lu.P;

            io.WriteLine("A=");
            io.WriteLine(lu.A.ToString(), 1);
            io.WriteLine("A^(-1)=");
            io.WriteLine(A_INVERSE.ToString(), 1);
            io.WriteLine("A * A^(-1) - E =");
            io.WriteLine(A_diff.ToString(), 1);
            io.SeparateText();

            //Нахождение норм
            double quadNorm = lu.A.QuadricNorm * A_INVERSE.QuadricNorm;
            double octNorm = lu.A.OctoNorm * A_INVERSE.OctoNorm;
            double eucNorm = lu.A.EuclideNorm;

            io.WriteLine("Cond(A) = ");
            io.WriteLine($"Quadratic norm = {quadNorm}", 1);
            io.WriteLine($"Octagon norm = {octNorm}", 1);
            io.WriteLine($"Euclidean norm = {eucNorm}", 1);
            io.WriteLine($"|A| = {lu.GetSign() * lu.L.Determinant}");
            io.SeparateText();

            //Нахождение невязки
            Vector B_diff = lu.A * _X - B.FastSwap(lu.m);

            io.WriteLine("A * X - B =");
            io.WriteLine(B_diff.ToString(), 1);
            io.WriteLine("Error = ");
            io.WriteLine((X - _X).ToString(), 1);

            io.FileClose();
        }

        static void IterativeMethods()
        {
            io.ExcelInit();

            double eps = 0.0001;
            int n;

            io.GetSize(out n);

            Matrix A = new Matrix(n, n);
            Vector B = new Vector(n);

            io.GetMatrixInfo(A, "A");
            io.GetVectorInfo(B, "B");

            io.TerminateExcel();
            io.FileOpen();

            io.WriteLine("INPUT");
            io.WriteLine("A=");
            io.WriteLine(A.ToString(), 1);
            io.WriteLine("B=");
            io.WriteLine(B.ToString(), 1);
            io.SeparateText();

            Methods.SSRM ssrm = new Methods.SSRM(A, B);

            io.WriteLine("Straight squareroot method result:");
            io.WriteLine(ssrm.Answer.ToString());
            io.WriteLine($"Matrix norm: {A.EuclideNorm}");
            io.SeparateText();

            io.WriteLine("Simple iteration method:");

            Methods.SIM sim = new Methods.SIM(A, B, eps, io);

            io.SeparateText();

            io.WriteLine("Fastest Gradient Descent method:");

            Methods.FGDM fgdm = new Methods.FGDM(A, B, eps, io);

            io.SeparateText();

            io.WriteLine("SOR method:");

            Methods.SOR sor = new Methods.SOR(A, B, ssrm.Answer, eps, io);

            io.SeparateText();

            io.WriteLine("Conjugate Gradient method:");

            Methods.CGM cgm = new Methods.CGM(A, B, ssrm.Answer, eps, io);

            io.SeparateText();

            io.WriteLine($"cond(A) = {A.EuclideCondition}");
            io.WriteLine("Theoretical number of iterations:");
            //io.WriteLine($"Simple iteration & Fastest Gradient Descent methods = {sim.Theoretical}");
            io.WriteLine($"SOR method = {sor.Theoretical}");
            io.WriteLine($"Conjugate Gradient method = {cgm.Theoretical}");

            io.FileClose();
        }
        static void NonlinearMethods() {
            io.FileOpen();

            Vector v = new Vector(2);
            v[0] = 0.9;
            v[1] = 2.6;
            NonlinearMethods.NM nm = new NonlinearMethods.NM(v, io);

            NonlinearMethods.SIM sim = new NonlinearMethods.SIM(nm.Answer, v);
            
        }

        static void TaskSwitch(int task) {
            switch ((Tasks)task) {
                case Tasks.StraightMethods: {
                        StraightMethods();
                    } break;

                case Tasks.IterativeMethods: {
                        IterativeMethods();
                    } break;

                case Tasks.NonlinearMethods: {
                    NonlinearMethods();
                } break;
                
                default: {
                        Console.WriteLine("Wrong program");
                    } break;
            }
        }

        static void Main(string[] args) {

            Console.ReadKey();

            io = new IOModule();

            Console.WriteLine("Choose program:");
            Console.WriteLine("1 - Straight Methods (Lab 1)");
            Console.WriteLine("2 - Iterative Methods (Lab 2)");
            Console.WriteLine("3 - Nonlinear Methods (Lab3)");
            Console.Write("Enter number: ");

            Console.WriteLine();

            int task;

            if (int.TryParse(Console.ReadLine(), out task)) {
                TaskSwitch(task);
            }

            Console.WriteLine("DONE!");
            Console.Read();
        }
    }
}
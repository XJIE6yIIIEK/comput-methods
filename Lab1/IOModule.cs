using System;
using System.IO;
using Matrices;
using Vectors;
using Excel = Microsoft.Office.Interop.Excel;


namespace Lab1 {
	enum InputType { 
		Matrix = 1,
		Vector = 2
	}

	/// <summary>
	/// Контроллер ввода-вывода
	/// </summary>
	internal class IOModule {
		//Excel блок
		private Excel.Application excelApp;
		private Excel.Workbook workbook;
		private bool excelTerminated = true;

		//Файловый блок
		private string outputFilePath;
		private StreamWriter writer;

		public IOModule() {
			outputFilePath = $@"{Directory.GetCurrentDirectory()}\results.txt";
		}

		/// <summary>
		/// Завершение Excel сессии
		/// </summary>
		public void TerminateExcel() {
			if(excelTerminated) {
				return;
			}

			excelTerminated = true;

			workbook.Close(0);
			excelApp.Quit();

			Console.WriteLine("EXCEL TERMINATED!!!!!");
		}

		/// <summary>
		/// Начало Excel сессии
		/// </summary>
		public void ExcelInit() {
			excelTerminated = false;

			excelApp = new Excel.Application();

			try {
				workbook = excelApp.Workbooks.Open($@"{Directory.GetCurrentDirectory()}\input.xlsx");
			} catch(Exception e) {
				TerminateExcel();
				throw new Exception(e.Message);
			}

			Console.WriteLine("EXCEL LAUNCHED!!!!!");
		}

		~IOModule() { 
			TerminateExcel();
		}

		/// <summary>
		/// Получение матрицы из Excel файла
		/// </summary>
		/// <param name="a">Матрица, в которую будет произведена запись</param>
		/// <param name="worksheet">Лист Excel книги, где содержится информация матрицы</param>
		private void GetMatrix(Matrix a, Excel.Worksheet worksheet) {
			Excel.Range lastCell = worksheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell, Type.Missing);
			int lastRow = lastCell.Row;
			int lastColumn = lastCell.Column;

			if(!(lastRow > 1 && lastColumn > 1)) {
				return;
			}

			Excel.Range data = worksheet.Range[worksheet.Cells[1, "A"], worksheet.Cells[lastRow, lastColumn]];

			for(int i = 1; i <= data.Rows.Count; i++) {
				for(int j = 1; j <= data.Columns.Count; j++) {
					a[i - 1, j - 1] = data.Cells[i, j].Value2;
				}
			}
		}

		/// <summary>
		/// Получение вектора из Excel файла
		/// </summary>
		/// <param name="b">Вектор, в который будет произведена запись</param>
		/// <param name="worksheet">Лист Excel книги, где содержится информация вектора</param>
		private void GetVector(Vector b, Excel.Worksheet worksheet) {
			Excel.Range lastCell = worksheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell, Type.Missing);
			int lastRow = lastCell.Row;

			if(!(lastRow > 1)) {
				return;
			}

			Excel.Range data = worksheet.Range[worksheet.Cells[1, "A"], worksheet.Cells[lastRow, "A"]];

			for(int i = 1; i <= data.Rows.Count; i++) {
				b[i - 1] = data.Cells[i, 1].Value2;
			}
		}

		public void GetMatrixInfo(Matrix A, string matrixName) {
			Excel.Worksheet AMatrixWorksheet = workbook.Worksheets[matrixName];
			GetMatrix(A, AMatrixWorksheet);
		}

		public void GetVectorInfo(Vector A, string vectorName) {
			Excel.Worksheet AVectorWorksheet = workbook.Worksheets[vectorName];
			GetVector(A, AVectorWorksheet);
		}

		/// <summary>
		/// Получение информации о размерности матриц и векторов
		/// </summary>
		/// <param name="n">Ссылка на параметр размерности вводимых векторов и матриц</param>
		public void GetSize(out int n) {
			Excel.Worksheet worksheet = workbook.Worksheets["Options"];
			Excel.Range lastCell = worksheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell, Type.Missing);
			int lastRow = lastCell.Row;
			Excel.Range settingsNameRange = worksheet.Range["A1", worksheet.Cells[lastRow, "A"]];

			n = (int)worksheet.Cells[settingsNameRange.Find("size").Row, "B"].Value2;
		}

		/// <summary>
		/// Открытие файлового потока
		/// </summary>
		public void FileOpen() {
			writer = new StreamWriter(outputFilePath, false);
		}

		/// <summary>
		/// Закрытие файлового потока
		/// </summary>
		public void FileClose() {
			writer.Close();
		}

		/// <summary>
		/// Запись строки в файл с последующим переносом
		/// </summary>
		/// <param name="str">Строка, которая будет записана в файл.</param>
		/// <param name="tab">Отступ строки. Каждый отступ включает в себя пробела.</param>
		public void WriteLine(string str = "", int tab = 0) {
			if(str == "") {
				writer.WriteLine();
				return;
			}

			if(tab != 0) {
				string tabStr = "\n";

				for(int i = 0; i < tab; i++) {
					tabStr = tabStr + ' ';
				}

				str = tabStr + str.Replace("\n", tabStr);
				str = str.Substring(1, str.Length - 1);
			}

			writer.WriteLine(str);
			Console.WriteLine(str);
		}

		/// <summary>
		/// Файловый сеператор для текста.
		/// </summary>
		public void SeparateText() {
			string sep = "\n================================================\n";
			writer.WriteLine(sep);
			Console.WriteLine(sep);
		}

		public string PrettyfyDouble(double num, int count) {
			if(Math.Abs(num) > 0.0000001) {
				return string.Format("{0," + count + ":0.000000;-0.000000;0}", num);
			} else {
				return string.Format("{0," + count + ":0.0000E0;-0.0000E0;0}", num);
			}
		}

		public string CenterString(string str, int width) {
			if(str.Length >= width) {
				return str;
			}

			int leftPadding = (width - str.Length) / 2;
			int rightPadding = width - str.Length - leftPadding;

			return new string(' ', leftPadding) + str + new string(' ', rightPadding);
		}
	}
}

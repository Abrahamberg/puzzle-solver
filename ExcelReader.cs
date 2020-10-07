using System;
using System.Collections.Generic;
using System.IO;
using Ardalis.GuardClauses;
using OfficeOpenXml;

namespace Puzzle
{
    public static class ExcelReader
    {
        public static IReadOnlyList<PuzzlePiece> Read(string filePath)
        {
            var results = new List<PuzzlePiece>();
            var existingFile = new FileInfo(filePath);
            using var package = new ExcelPackage(existingFile);
            // get the first worksheet in the workbook
            var worksheet = package.Workbook.Worksheets[0];
            var strRows = worksheet.Cells[1, 1].Value.ToString();
            Guard.Against.NullOrWhiteSpace(strRows,
                "You need to support the number or rows in the first cell of excel file");

            if (!int.TryParse(strRows, out var numberOfRows))
                throw new Exception("Number of puzzle pieces in in the first cell of excel file  should be an integer ");

            for (var i = 2; i <= numberOfRows + 1; i++)
            {
                var cell = worksheet.Cells[i, 1].Value.ToString();
                Guard.Against.NullOrWhiteSpace(cell, $"Cell [1,{i}] cannot be empty");
                results.Add(new PuzzlePiece(cell));
            }

            return results.AsReadOnly();
        }
    }
}
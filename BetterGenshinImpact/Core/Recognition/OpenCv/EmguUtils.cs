using System;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Mat = Emgu.CV.Mat;
using Point = System.Drawing.Point;


namespace BetterGenshinImpact.Core.Recognition.OpenCv;

public static class EmguUtils
{
    public static void CvRectangle(IInputOutputArray img, Rectangle rect, MCvScalar color, int thickness = 1,
        LineType lineType = LineType.EightConnected, int shift = 0)
    {
        CvInvoke.Rectangle(img, rect, color, thickness, lineType, shift);
    }

    public static void CvRectangle(
        IInputOutputArray img,
        Point pt1,
        Point pt2,
        MCvScalar color,
        int thickness = 1,
        LineType lineType = LineType.EightConnected,
        int shift = 0)
    {
        CvRectangle(img, Point2Rectangle(pt1, pt2), color, thickness, lineType, shift);
    }

    private static Rectangle Point2Rectangle(Point pt1, Point pt2)
    {
        var left = Math.Min(pt1.X, pt2.X);
        var top = Math.Min(pt1.Y, pt2.Y);
        var right = Math.Max(pt1.X, pt2.X);
        var bottom = Math.Max(pt1.Y, pt2.Y);
        return Rectangle.FromLTRB(left, top, right, bottom);
    }

    public static void CvMinMaxLoc(
        IInputArray src,
        out double minVal,
        out double maxVal,
        out Point minLoc,
        out Point maxLoc,
        IInputArray? mask = null)
    {
        var minValref = 0.0;
        var maxValref = 0.0;
        var minLocref = new Point();
        var maxLocref = new Point();
        CvInvoke.MinMaxLoc(src, ref minValref, ref maxValref, ref minLocref, ref maxLocref, mask);
        minVal = minValref;
        maxVal = maxValref;
        minLoc = minLocref;
        maxLoc = maxLocref;
    }

    public static Mat CvNewMat(Mat m, Rectangle rectangle)
    {
        return new Mat(m, rectangle);
    }

    public static Mat CvNewMat(int rows, int cols, DepthType type, MCvScalar s)
    {
        return CvNewMat(rows, cols, type, 1, s);
    }

    public static Mat CvNewMat(int rows, int cols, DepthType type, int channels, MCvScalar s, int step = 0)
    {
        return new Mat(rows, cols, type, channels, new ScalarArray(s), step);
    }

    public static void CvRectangle(this Mat mat, Rectangle rect, MCvScalar color, int thickness = 1,
        LineType lineType = LineType.EightConnected, int shift = 0)
    {
        CvInvoke.Rectangle(mat, rect, color, thickness, lineType, shift);
    }

    public static MCvScalar CvToScalar(this Color color)
    {
        return new MCvScalar(color.R, color.G, color.B);
    }

    public static ScalarArray CvScalarToArray(this MCvScalar scalar)
    {
        return new ScalarArray(scalar);
    }

    public static void CvFindContours(
        IInputOutputArray image,
        out VectorOfVectorOfPoint contours,
        out Mat hierarchy, // 其实这是个Vec4i，Cv32S，4通道
        RetrType mode,
        ChainApproxMethod method,
        Point? offset = null)
    {
        CvInvoke.FindContours(image, contours = new VectorOfVectorOfPoint(), hierarchy = new Mat(), mode, method,
            offset ?? Point.Empty);
    }


    public static void CvDilate(
        IInputArray src,
        IOutputArray dst,
        IInputArray? element,
        Point? anchor = null,
        int iterations = 1,
        BorderType borderType = BorderType.Constant,
        MCvScalar? borderValue = null)
    {
        CvInvoke.Dilate(src, dst, element, anchor ?? new Point(-1, -1), iterations, borderType,
            borderValue ?? new MCvScalar(double.MaxValue, double.MaxValue, double.MaxValue, double.MaxValue));
    }
}
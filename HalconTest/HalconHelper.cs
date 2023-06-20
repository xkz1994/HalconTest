using HalconDotNet;
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace HalconTest
{
    public static class HalconHelper
    {

        /// <summary>
        /// HObject转为彩色图
        /// </summary>
        /// <param name="ho_image"></param>
        /// <param name="res24"></param>
        /// 
        public static void Bitmap2HObjectBpp8(Bitmap bmp, out HObject image)
        {
            try
            {
                Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
                BitmapData srcBmpData = bmp.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format8bppIndexed);
                HOperatorSet.GenImage1(out image, "byte", bmp.Width, bmp.Height, srcBmpData.Scan0);
                bmp.UnlockBits(srcBmpData);
            }
            catch (Exception ex)
            {
                image = null;
            }
            finally
            {
                bmp.Dispose();
            }
        }

        public static void findCircle(HObject ho_Image, out HTuple hv_Radius, out HTuple hv_offsetx,
     out HTuple hv_offsety)
        {



            // Local iconic variables 

            HObject ho_ImageZoom = null, ho_ImageMirror1 = null;
            HObject ho_ImageMirror2 = null, ho_ImageMirror3 = null, ho_ImageResult1 = null;
            HObject ho_ImageResult2 = null, ho_ImageResult = null, ho_Rectangle = null;
            HObject ho_ImageReduced = null, ho_Region = null, ho_RegionClosing = null;
            HObject ho_RegionOpening = null, ho_ConnectedRegions = null;
            HObject ho_SelectedRegions = null, ho_Contours = null, ho_ContCircle = null;

            // Local control variables 

            HTuple hv_debugFlag = new HTuple(), hv_Width = new HTuple();
            HTuple hv_Height = new HTuple(), hv_Area = new HTuple();
            HTuple hv_Row = new HTuple(), hv_Column = new HTuple();
            HTuple hv_Max = new HTuple(), hv_Indices = new HTuple();
            HTuple hv_Row1 = new HTuple(), hv_Column1 = new HTuple();
            HTuple hv_StartPhi = new HTuple(), hv_EndPhi = new HTuple();
            HTuple hv_PointOrder = new HTuple(), hv_Exception = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_ImageZoom);
            HOperatorSet.GenEmptyObj(out ho_ImageMirror1);
            HOperatorSet.GenEmptyObj(out ho_ImageMirror2);
            HOperatorSet.GenEmptyObj(out ho_ImageMirror3);
            HOperatorSet.GenEmptyObj(out ho_ImageResult1);
            HOperatorSet.GenEmptyObj(out ho_ImageResult2);
            HOperatorSet.GenEmptyObj(out ho_ImageResult);
            HOperatorSet.GenEmptyObj(out ho_Rectangle);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_Region);
            HOperatorSet.GenEmptyObj(out ho_RegionClosing);
            HOperatorSet.GenEmptyObj(out ho_RegionOpening);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
            HOperatorSet.GenEmptyObj(out ho_Contours);
            HOperatorSet.GenEmptyObj(out ho_ContCircle);
            hv_Radius = new HTuple();
            hv_offsetx = new HTuple();
            hv_offsety = new HTuple();
            try
            {
                try
                {
                    hv_debugFlag.Dispose();
                    hv_debugFlag = 1;
                    hv_Width.Dispose(); hv_Height.Dispose();
                    HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                    ho_ImageZoom.Dispose();
                    HOperatorSet.ZoomImageSize(ho_Image, out ho_ImageZoom, hv_Width, hv_Width,
                        "constant");
                    ho_ImageMirror1.Dispose();
                    HOperatorSet.MirrorImage(ho_ImageZoom, out ho_ImageMirror1, "row");
                    ho_ImageMirror2.Dispose();
                    HOperatorSet.MirrorImage(ho_ImageZoom, out ho_ImageMirror2, "column");
                    ho_ImageMirror3.Dispose();
                    HOperatorSet.MirrorImage(ho_ImageZoom, out ho_ImageMirror3, "diagonal");
                    ho_ImageResult1.Dispose();
                    HOperatorSet.AddImage(ho_ImageMirror2, ho_ImageMirror1, out ho_ImageResult1,
                        0.5, 0);
                    ho_ImageResult2.Dispose();
                    HOperatorSet.AddImage(ho_ImageMirror3, ho_ImageZoom, out ho_ImageResult2,
                        0.5, 0);
                    ho_ImageResult.Dispose();
                    HOperatorSet.AddImage(ho_ImageResult2, ho_ImageResult1, out ho_ImageResult,
                        0.5, 0);
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        ho_Rectangle.Dispose();
                        HOperatorSet.GenRectangle1(out ho_Rectangle, 30, 30, hv_Width - 30, hv_Width - 30);
                    }
                    ho_ImageReduced.Dispose();
                    HOperatorSet.ReduceDomain(ho_ImageResult, ho_Rectangle, out ho_ImageReduced
                        );
                    ho_Region.Dispose();
                    HOperatorSet.Threshold(ho_ImageReduced, out ho_Region, 0, 30);
                    ho_RegionClosing.Dispose();
                    HOperatorSet.ClosingCircle(ho_Region, out ho_RegionClosing, 5.5);
                    ho_RegionOpening.Dispose();
                    HOperatorSet.OpeningCircle(ho_RegionClosing, out ho_RegionOpening, 5.5);
                    ho_ConnectedRegions.Dispose();
                    HOperatorSet.Connection(ho_RegionOpening, out ho_ConnectedRegions);
                    hv_Area.Dispose(); hv_Row.Dispose(); hv_Column.Dispose();
                    HOperatorSet.AreaCenter(ho_ConnectedRegions, out hv_Area, out hv_Row, out hv_Column);
                    hv_Max.Dispose();
                    HOperatorSet.TupleMax(hv_Area, out hv_Max);
                    hv_Indices.Dispose();
                    HOperatorSet.TupleFind(hv_Area, hv_Max, out hv_Indices);
                    ho_SelectedRegions.Dispose();
                    HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions, "area",
                        "and", hv_Max, hv_Max);
                    ho_Contours.Dispose();
                    HOperatorSet.GenContourRegionXld(ho_SelectedRegions, out ho_Contours, "border");
                    hv_Row1.Dispose(); hv_Column1.Dispose(); hv_Radius.Dispose(); hv_StartPhi.Dispose(); hv_EndPhi.Dispose(); hv_PointOrder.Dispose();
                    HOperatorSet.FitCircleContourXld(ho_Contours, "algebraic", -1, 0, 0, 3, 2,
                        out hv_Row1, out hv_Column1, out hv_Radius, out hv_StartPhi, out hv_EndPhi,
                        out hv_PointOrder);
                    hv_offsetx.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_offsetx = hv_Column1 - (hv_Width * 0.5);
                    }
                    hv_offsety.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_offsety = ((hv_Row1 * hv_Height) / hv_Width) - (hv_Height * 0.5);
                    }


                    if ((int)(hv_debugFlag) != 0)
                    {
                        ho_ContCircle.Dispose();
                        HOperatorSet.GenCircleContourXld(out ho_ContCircle, hv_Row1, hv_Column1,
                            hv_Radius, hv_StartPhi, hv_EndPhi, "positive", 1);
                        if (HDevWindowStack.IsOpen())
                        {
                            HOperatorSet.DispObj(ho_Image, HDevWindowStack.GetActive());
                        }
                        if (HDevWindowStack.IsOpen())
                        {
                            HOperatorSet.DispObj(ho_ContCircle, HDevWindowStack.GetActive());
                        }
                        // stop(...); only in hdevelop
                    }
                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                    hv_offsetx.Dispose();
                    hv_offsetx = 0;
                    hv_offsety.Dispose();
                    hv_offsety = 0;
                    hv_Radius.Dispose();
                    hv_Radius = 0;
                }

                ho_ImageZoom.Dispose();
                ho_ImageMirror1.Dispose();
                ho_ImageMirror2.Dispose();
                ho_ImageMirror3.Dispose();
                ho_ImageResult1.Dispose();
                ho_ImageResult2.Dispose();
                ho_ImageResult.Dispose();
                ho_Rectangle.Dispose();
                ho_ImageReduced.Dispose();
                ho_Region.Dispose();
                ho_RegionClosing.Dispose();
                ho_RegionOpening.Dispose();
                ho_ConnectedRegions.Dispose();
                ho_SelectedRegions.Dispose();
                ho_Contours.Dispose();
                ho_ContCircle.Dispose();

                hv_debugFlag.Dispose();
                hv_Width.Dispose();
                hv_Height.Dispose();
                hv_Area.Dispose();
                hv_Row.Dispose();
                hv_Column.Dispose();
                hv_Max.Dispose();
                hv_Indices.Dispose();
                hv_Row1.Dispose();
                hv_Column1.Dispose();
                hv_StartPhi.Dispose();
                hv_EndPhi.Dispose();
                hv_PointOrder.Dispose();
                hv_Exception.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_ImageZoom.Dispose();
                ho_ImageMirror1.Dispose();
                ho_ImageMirror2.Dispose();
                ho_ImageMirror3.Dispose();
                ho_ImageResult1.Dispose();
                ho_ImageResult2.Dispose();
                ho_ImageResult.Dispose();
                ho_Rectangle.Dispose();
                ho_ImageReduced.Dispose();
                ho_Region.Dispose();
                ho_RegionClosing.Dispose();
                ho_RegionOpening.Dispose();
                ho_ConnectedRegions.Dispose();
                ho_SelectedRegions.Dispose();
                ho_Contours.Dispose();
                ho_ContCircle.Dispose();

                hv_debugFlag.Dispose();
                hv_Width.Dispose();
                hv_Height.Dispose();
                hv_Area.Dispose();
                hv_Row.Dispose();
                hv_Column.Dispose();
                hv_Max.Dispose();
                hv_Indices.Dispose();
                hv_Row1.Dispose();
                hv_Column1.Dispose();
                hv_StartPhi.Dispose();
                hv_EndPhi.Dispose();
                hv_PointOrder.Dispose();
                hv_Exception.Dispose();

                throw HDevExpDefaultException;
            }
        }

    }
}

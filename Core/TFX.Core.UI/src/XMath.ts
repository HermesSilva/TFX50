class XMath
{
    private static m_w = 123456789;
    private static m_z = 987654321;
    private static mask = 0xffffffff;

    //static AddCorner(pCorner: XPoint, pRound: number, pOut1: XPoint, pOut2: XPoint): XArray<XPoint>
    //{
    //    if (!pCorner.Equals(pOut1) && !pCorner.Equals(pOut2) && ((pOut1.Y == pCorner.Y && pCorner.X == pOut2.X) || (pOut1.X == pCorner.X && pCorner.Y == pOut2.Y)))
    //    {
    //        let x1 = pCorner.X == pOut1.X ? pRound * 2 : Math.abs(pCorner.X - pOut1.X);
    //        let y1 = pCorner.Y == pOut1.Y ? pRound * 2 : Math.abs(pCorner.Y - pOut1.Y);
    //        let x2 = pCorner.X == pOut2.X ? pRound * 2 : Math.abs(pCorner.X - pOut2.X);
    //        let y2 = pCorner.Y == pOut2.Y ? pRound * 2 : Math.abs(pCorner.Y - pOut2.Y);
    //        let size = Math.min(Math.min(x1, y1), Math.min(x2, y2));
    //        pRound = size / 2;
    //    }
    //    else
    //        pRound = 0;
    //    pOut1 = XMath.PointCircle(pCorner, pOut1, pRound);
    //    pOut2 = XMath.PointCircle(pCorner, pOut2, pRound);
    //    return [pOut1, pCorner, pOut2];
    //}

    static CreateArrow(pt: XPoint, pt2: XPoint, pWidth: number): XArray<XPoint>
    {
        let dg = this.AngleInRad(pt, pt2) / Math.PI * 180 - 90;
        let ln1 = XMath.RotatePoints(pt, [pt, new XPoint(pt.X + pWidth, pt.Y)], dg - 22.5);
        let ln2 = XMath.RotatePoints(pt, ln1, 45);
        return [ln1[0], ln1[1], ln2[1], ln2[0]];
    }

    static RotatePoints(pCenter: XPoint, pPoints: XPoint[], pDegree: number): XArray<XPoint>
    {
        let ret = new XArray<XPoint>();
        for (var i = 0; i < pPoints.length; i++)
            ret.Add(XMath.RotatePoint(pCenter, pPoints[i], pDegree));
        return ret;
    }

    static RotatePoint(pCenter: XPoint, pPoint: XPoint, pDegree: number): XPoint
    {
        let length = XMath.Distance2Points(pPoint, pCenter);
        let degree = XMath.AngleInRad(pPoint, pCenter) + ((pDegree * Math.PI) / 180);
        return new XPoint(pCenter.X - (length * Math.sin(degree)), pCenter.Y - (length * Math.cos(degree)));
    }

    static Round(pRect: XRect, pFactor: number): XRect
    {
        return new XRect(XMath.RoundN(pRect.Left, pFactor), XMath.RoundN(pRect.Top, pFactor), XMath.RoundN(pRect.Width, pFactor), XMath.RoundN(pRect.Height, pFactor));
    }

    static RoundN(pValue: number, pFactor: number): number
    {
        if (Number.isNaN(pValue))
            return 0;
        var v = Math.floor(pValue / pFactor);
        return v * pFactor;
    }

    static Distance2Points(pPoint: XPoint, pCenter: XPoint): number
    {
        return Math.sqrt(Math.pow(pPoint.X - pCenter.X, 2) + Math.pow(pPoint.Y - pCenter.Y, 2));
    }

    static LineIntersectsRect(pRect: XRect, p1: XPoint, p2: XPoint): boolean
    {
        return XMath.LineIntersectsLine(p1, p2, new XPoint(pRect.X, pRect.Y), new XPoint(pRect.X + pRect.Width, pRect.Y)) ||
            XMath.LineIntersectsLine(p1, p2, new XPoint(pRect.X + pRect.Width, pRect.Y), new XPoint(pRect.X + pRect.Width, pRect.Y + pRect.Height)) ||
            XMath.LineIntersectsLine(p1, p2, new XPoint(pRect.X + pRect.Width, pRect.Y + pRect.Height), new XPoint(pRect.X, pRect.Y + pRect.Height)) ||
            XMath.LineIntersectsLine(p1, p2, new XPoint(pRect.X, pRect.Y + pRect.Height), new XPoint(pRect.X, pRect.Y)) ||
            (pRect.Contains(p1) && pRect.Contains(p2));
    }

    private static LineIntersectsLine(l1p1: XPoint, l1p2: XPoint, l2p1: XPoint, l2p2: XPoint): boolean
    {
        var q = (l1p1.Y - l2p1.Y) * (l2p2.X - l2p1.X) - (l1p1.X - l2p1.X) * (l2p2.Y - l2p1.Y);
        var d = (l1p2.X - l1p1.X) * (l2p2.Y - l2p1.Y) - (l1p2.Y - l1p1.Y) * (l2p2.X - l2p1.X);
        if (d == 0)
            return false;
        var r = q / d;
        q = (l1p1.Y - l2p1.Y) * (l1p2.X - l1p1.X) - (l1p1.X - l2p1.X) * (l1p2.Y - l1p1.Y);
        var s = q / d;
        if (r < 0 || r > 1 || s < 0 || s > 1)
            return false;
        return true;
    }

    static LineIntersection(pP1Line1: XPoint, pP2Line1: XPoint, pP1Line2: XPoint, pP2Line2: XPoint): XPoint
    {
        var dx1 = pP2Line1.X - pP1Line1.X;
        var dy1 = pP2Line1.Y - pP1Line1.Y;
        var dx2 = pP2Line2.X - pP1Line2.X;
        var dy2 = pP2Line2.Y - pP1Line2.Y;
        var det = (dx2 * dy1) - (dy2 * dx1);
        if (det == 0)
            return new XPoint(Number.NaN, Number.NaN);
        var mu = (((pP1Line1.X - pP1Line2.X) * dy1) - ((pP1Line1.Y - pP1Line2.Y) * dx1)) / det;
        var mu2 = (((pP1Line1.X - pP1Line2.X) * dy2) - ((pP1Line1.Y - pP1Line2.Y) * dx2)) / det;
        if (mu >= 0 && mu <= 1 && mu2 >= 0 && mu2 <= 1)
            return new XPoint(pP1Line2.X + (mu * dx2), pP1Line2.Y + (mu * dy2));
        return new XPoint(Number.NaN, Number.NaN);
    }

    static ToPolygonEx(pRect: XRect, pInflateLine: number = 0): XArray<XArray<XPoint>>
    {
        var list = new XArray<XArray<XPoint>>();
        list[0] = [new XPoint(pRect.Left - pInflateLine, pRect.Top), new XPoint(pRect.Right + pInflateLine, pRect.Top)];
        list[1] = [new XPoint(pRect.Right, pRect.Top - pInflateLine), new XPoint(pRect.Right, pRect.Bottom + pInflateLine)];
        list[2] = [new XPoint(pRect.Right + pInflateLine, pRect.Bottom), new XPoint(pRect.Left - pInflateLine, pRect.Bottom)];
        list[3] = [new XPoint(pRect.Left, pRect.Bottom + pInflateLine), new XPoint(pRect.Left, pRect.Top - pInflateLine)];
        return list;
    }

    static AddCorner(pCorner: XPoint, pRound: number, pP1: XPoint, pP2: XPoint): XArray<XPoint>
    {
        if (!pCorner.Equals(pP1) && !pCorner.Equals(pP2) && ((Math.floor(pP1.Y) == Math.floor(pCorner.Y) && Math.floor(pCorner.X) == Math.floor(pP2.X)) ||
            (Math.floor(pP1.X) == Math.floor(pCorner.X) && Math.floor(pCorner.Y) == Math.floor(pP2.Y))))
        {
            var x1 = pCorner.X == pP1.X ? pRound * 2 : Math.abs(pCorner.X - pP1.X);
            var y1 = pCorner.Y == pP1.Y ? pRound * 2 : Math.abs(pCorner.Y - pP1.Y);
            var x2 = pCorner.X == pP2.X ? pRound * 2 : Math.abs(pCorner.X - pP2.X);
            var y2 = pCorner.Y == pP2.Y ? pRound * 2 : Math.abs(pCorner.Y - pP2.Y);
            var size = Math.min(Math.min(x1, y1), Math.min(x2, y2));
            pRound = size / 2;
        }
        else
            pRound = 0;
        return [XMath.PointCircle(pCorner, pP1, pRound), pCorner, XMath.PointCircle(pCorner, pP2, pRound)];
    }

    static PointCircle(pCenter: XPoint, pPoint: XPoint, pRadiusX: number, pRadiusY: number = -1): XPoint
    {
        if (pRadiusY == -1)
            pRadiusY = pRadiusX;
        var dg = XMath.AngleInRad(pCenter, pPoint) + Math.PI;
        return new XPoint(pCenter.X - (pRadiusX * Math.sin(dg)), pCenter.Y - (pRadiusY * Math.cos(dg)));
    }

    static AngleInRad(pFirst: XPoint, pSecond: XPoint): number
    {
        var degree = 0;
        if (pFirst.X == pSecond.X)
            if (pFirst.Y < pSecond.Y)
                degree = Math.PI * 1.5;
            else
                degree = Math.PI / 2.0;
        else
            degree = Math.atan((pSecond.Y - pFirst.Y) / (pFirst.X - pSecond.X));
        if (pSecond.X < pFirst.X)
            degree = degree + Math.PI;
        degree = degree + (Math.PI / 2.0);
        return degree;
    }

    static PolarToCartesian(pCenter: XPoint, pRadius: number, pDegrees: number): XPoint
    {
        var rad = (pDegrees - 90) * Math.PI / 180.0;
        return new XPoint(pCenter.X + (pRadius * Math.cos(rad)), pCenter.Y + (pRadius * Math.sin(rad)));
    }

    static DonutSlice(pCenter: XPoint, pRadius: number, pStartDegrees: number, pEndDegrees: number, pWidth: number)
    {
        var start = XMath.PolarToCartesian(pCenter, pRadius, pEndDegrees);
        var end = XMath.PolarToCartesian(pCenter, pRadius, pStartDegrees);
        var iradius = pRadius - pWidth;
        var istart = XMath.PolarToCartesian(pCenter, iradius, pEndDegrees);
        var iend = XMath.PolarToCartesian(pCenter, iradius, pStartDegrees);
        var flag = pEndDegrees - pStartDegrees <= 180 ? "0" : "1";
        var x = "";
        var d = ["M", start.X, start.Y, "A", pRadius, pRadius, 0, flag, 0, end.X, end.Y, "L", iend.X, iend.Y, "A", iradius, iradius, 0, flag, 1, istart.X, istart.Y, "Z", x].join(" ");
        return d;
    }

    static PieSlice(pCenter: XPoint, pRadius: number, pStartDegrees: number, pEndDegrees: number)
    {
        var start = XMath.PolarToCartesian(pCenter, pRadius, pEndDegrees);
        var end = XMath.PolarToCartesian(pCenter, pRadius, pStartDegrees);
        var flag = pEndDegrees - pStartDegrees <= 180 ? "0" : "1";
        var d = ["M", pCenter.X, pCenter.Y, "L", start.X, start.Y, "A", pRadius, pRadius, 0, flag, 0, end.X, end.Y, "L", pCenter.X, pCenter.Y].join(" ");
        return d;
    }

    static Arc(pCenter: XPoint, pRadius: number, pStartDegrees: number, pEndDegrees: number)
    {
        var start = XMath.PolarToCartesian(pCenter, pRadius, pEndDegrees);
        var end = XMath.PolarToCartesian(pCenter, pRadius, pStartDegrees);
        var flag = pEndDegrees - pStartDegrees <= 180 ? "0" : "1";
        var d = ["M", start.X, start.Y, "A", pRadius, pRadius, 0, flag, 0, end.X, end.Y].join(" ");
        return d;
    }

    static Seed(pSeed: number = -1)
    {
        if (pSeed == -1)
            pSeed = new Date().getTime();
        XMath.m_w = (123456789 + pSeed) & XMath.mask;
        XMath.m_z = (987654321 - pSeed) & XMath.mask;
    }

    static Random()
    {
        XMath.m_z = (36969 * (XMath.m_z & 65535) + (XMath.m_z >> 16)) & XMath.mask;
        XMath.m_w = (18000 * (XMath.m_w & 65535) + (XMath.m_w >> 16)) & XMath.mask;
        var result = ((XMath.m_z << 16) + (XMath.m_w & 65535)) >>> 0;
        result /= 4294967296;
        return result;
    }
}
interface XSortCompare<T> { (pLeft: T, pRight: T): number; }

interface XSortSwap<T> { (pArray: Array<T>, pLeft: number, pRight: number): void; }

class XSort
{
    public static Sort<T>(pArray: Array<T>, pSwap: XSortSwap<T>, pComparer: XSortCompare<T>, pOwner: any): Array<T>
    {
        XSort.QuickSort<T>(pArray, 0, pArray.length - 1, pSwap, pComparer, pOwner);
        return pArray;
    }

    private static QuickSort<T>(map: Array<T>, left: number, right: number, pSwap: XSortSwap<T>, pComparer: XSortCompare<T>, pOwner: any)
    {
        do
        {
            var i = left;
            var j = right;
            var elm = map[i + ((j - i) >> 1)];
            do
            {
                while (i < map.length && pComparer.call(pOwner, elm, map[i]) > 0) i++;
                while (j >= 0 && pComparer.call(pOwner, elm, map[j]) < 0) j--;
                if (i > j)
                    break;
                if (i < j)
                    pSwap.call(pOwner, map, i, j);
                i++;
                j--;
            } while (i <= j);
            if (j - left <= right - i)
            {
                if (left < j)
                    XSort.QuickSort.call(pOwner, map, left, j, <any>pSwap, <any>pComparer, pOwner);
                left = i;
            }
            else
            {
                if (i < right)
                    XSort.QuickSort.call(pOwner, map, i, right, <any>pSwap, <any>pComparer, pOwner);
                right = j;
            }
        } while (left < right);
    }

    public static Swap<T>(pArray: Array<T>, pLeft: number, pRight: number)
    {
        var tmp = pArray[pLeft];
        pArray[pLeft] = pArray[pRight];
        pArray[pRight] = tmp;
    }
}
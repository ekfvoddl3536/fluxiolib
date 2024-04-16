﻿using fluxiolib.Internal;
using System.Diagnostics;

namespace fluxiolib;

/// <summary>
/// 검색할 필드의 범위를 설정합니다.
/// </summary>
#if DEBUG
[DebuggerDisplay("start: {start}, count: {count}, isConstrained: {IsConstrained}")]
#endif
[StructLayout(LayoutKind.Sequential, Pack = sizeof(long))]
public readonly struct FluxSearchSpace
{
    public readonly int start;
    public readonly int b_isConstrained;
    public readonly int count;

    /// <summary>
    /// 새로운 <see cref="FluxSearchSpace"/>를 초기화합니다.
    /// </summary>
    /// <param name="start">검색을 시작할 위치입니다.</param>
    /// <param name="count"><paramref name="start"/>위치 부터 검색할 개수입니다. 0을 지정하면, 자동으로 개수를 설정합니다.</param>
    /// <param name="isConstrained">
    /// 상위 유형(Type)으로 확장하여 검색하는 것에 제약을 사용할 지 여부입니다.
    /// True를 지정하면, 현재 유형에서만 검색을 수행합니다.
    /// </param>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public FluxSearchSpace(Index start, int count, bool isConstrained = false)
    {
        this.start = Unsafe.As<Index, int>(ref start);
        this.count = count;

        b_isConstrained = isConstrained ? 1 : 0;
    }

    /// <summary>
    /// 새로운 <see cref="FluxSearchSpace"/>를 초기화합니다.
    /// </summary>
    /// <param name="start">검색을 시작할 위치입니다.</param>
    /// <param name="isConstrained">
    /// 상위 유형(Type)으로 확장하여 검색하는 것에 제약을 사용할 지 여부입니다.
    /// True를 지정하면, 현재 유형에서만 검색을 수행합니다.
    /// </param>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public FluxSearchSpace(Index start, bool isConstrained = false)
        : this(start, 0, isConstrained)
    {
    }

    public bool IsConstrained => b_isConstrained != 0;

    [SkipLocalsInit, MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    internal unsafe (uint, uint) GetOffsetAndLength(scoped ref readonly MethodTable pMT)
    {
        Debug.Assert(count >= 0);

        uint numFields = MethodTable.GetNumInstanceFields(in pMT);

        uint offset = (uint)start;
        uint temp =
            pMT.pParentMT == null
            ? 0
            : offset + EEClass.getValue(&pMT.pParentMT->pEEClass->numInstanceFields);

        temp =
            b_isConstrained != 0
            ? temp
            : offset;

        uint v2 = v2 = offset + numFields + 1;

        offset =
            (int)offset < 0
            ? v2
            : temp;

        temp = numFields - offset;

        numFields = (uint)count;
        numFields =
            numFields == 0
            ? temp
            : numFields;

        return (offset, numFields);
    }
}

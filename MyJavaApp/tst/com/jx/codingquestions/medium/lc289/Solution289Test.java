package com.jx.codingquestions.medium.lc289;

import static org.junit.Assert.assertArrayEquals;

import java.util.AbstractMap;
import java.util.Arrays;
import java.util.List;
import java.util.Map;
import java.util.stream.Collectors;
import java.util.stream.Stream;

import org.junit.Test;

public class Solution289Test {

    private static final List<Map.Entry<int[][], int[][]>> INPUT_TO_OUTPUT_MAP = Stream.of(
        new AbstractMap.SimpleEntry<>(
            new int[][] {
                new int[] { 0, 1, 0 },
                new int[] { 0, 0, 1 },
                new int[] { 1, 1, 1 },
                new int[] { 0, 0, 0 }
            },
            new int[][] {
                new int[] { 0, 0, 0 },
                new int[] { 1, 0, 1 },
                new int[] { 0, 1, 1 },
                new int[] { 0, 1, 0 }
            })
    ).collect(Collectors.<Map.Entry<int[][], int[][]>>toList());

    private final Solution289 subject = new Solution289();
    private final Solution289NotInplace subjectNotInplace = new Solution289NotInplace();


    @Test
    public void test() {

        for(final Map.Entry<int[][], int[][]> pair : INPUT_TO_OUTPUT_MAP) {
            final int[][] notInplaceActual = subjectNotInplace.gameOfLife(pair.getKey());

            int[][] inplaceActual = pair.getKey().clone();
            subject.gameOfLife(inplaceActual);

            System.out.printf(
                "input: \t\t\t\t%s\n"
                    + "not_inplace output: %s\n"
                    + "inplace output: \t%s\n"
                    + "expected output: \t%s\n\n",
                Arrays.deepToString(pair.getKey()),
                Arrays.deepToString(notInplaceActual),
                Arrays.deepToString(inplaceActual),
                Arrays.deepToString(pair.getValue()));
            assertArrayEquals(pair.getValue(), notInplaceActual);
            assertArrayEquals(pair.getValue(), inplaceActual);
        }
    }
}

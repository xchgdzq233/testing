package com.jx.leetcode.easy;

import static org.hamcrest.CoreMatchers.equalTo;
import static org.hamcrest.MatcherAssert.assertThat;

import org.junit.Before;
import org.junit.Test;

public class Lc1047Test {

    private Solution1047 solution1047;

    @Before
    public void setupClass() {

        solution1047 = new Solution1047();
    }

    private void runAssertion(final String input, final String expectedOutput) {

        final String actualOutput = solution1047.applyWithStack(input);

        assertThat(actualOutput, equalTo(expectedOutput));
        assertThat(solution1047.applyWithStringBuffer(input), equalTo(expectedOutput));
        assertThat(solution1047.applyWithTwoPointers(input), equalTo(expectedOutput));

        System.out.printf("%s -> %s\n", quotationMarksIfEmptyString(input), quotationMarksIfEmptyString(actualOutput));
    }

    private String quotationMarksIfEmptyString(final String input) {

        return "".equals(input) ? "\"\"" : input;
    }

    @Test
    public void tests() {

        runAssertion("abbaca", "ca");
        runAssertion("abcba", "abcba");
        runAssertion("", "");
        runAssertion("abcdefggfedcba", "");
        runAssertion("abbbaccab", "a");
    }
}

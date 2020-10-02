package com.jx.leetcode.easy;

import static org.hamcrest.CoreMatchers.equalTo;
import static org.hamcrest.MatcherAssert.assertThat;

import org.junit.Before;
import org.junit.Test;

public class Lc1047Test {

    private Lc1047 lc1047;

    private String input;
    private String expectedOutput;

    @Before
    public void setupClass() {

        lc1047 = new Lc1047();

        input = "";
        expectedOutput = "";
    }

    private void runAssertion() {

        assertThat(lc1047.applyWithStack(input), equalTo(expectedOutput));
        assertThat(lc1047.applyWithStringBuffer(input), equalTo(expectedOutput));
        assertThat(lc1047.applyWithTwoPointers(input), equalTo(expectedOutput));
    }

    @Test
    public void test_sample() {

        input = "abbaca";
        expectedOutput = "ca";
        runAssertion();
    }

    @Test
    public void test_emptyInput() {

        input = "";
        expectedOutput = "";
        runAssertion();
    }

    @Test
    public void test_removeAll() {

        input = "abcdefggfedcba";
        expectedOutput = "";
        runAssertion();
    }

    @Test
    public void test_triples() {

        input = "abbbaccab";
        expectedOutput = "a";
        runAssertion();
    }
}
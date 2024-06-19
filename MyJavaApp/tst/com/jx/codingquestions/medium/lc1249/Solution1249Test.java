package com.jx.codingquestions.medium.lc1249;

import java.util.Arrays;
import java.util.Collections;
import java.util.HashSet;
import java.util.Map;
import java.util.Set;

import org.junit.Before;
import org.junit.Test;

public class Solution1249Test {

    private Solution1249 solution1249;

    @Before
    public void setup() {

        solution1249 = new Solution1249();
    }

    @Test
    public void test() {

        final Map<String, Set<String>> inputToOutput = Map.of(
                "lee(t(c)o)de)", new HashSet<>(Arrays.asList("lee(t(c)o)de", "lee(t(co)de)", "lee(t(c)ode)")),
                "a)b(c)d", new HashSet<>(Collections.singletonList("ab(c)d")),
                "))((", new HashSet<>(Collections.singletonList(""))
        );
        for (final Map.Entry<String, Set<String>> entry : inputToOutput.entrySet()) {
            final String input = entry.getKey();
            final Set<String> expectedOutput = entry.getValue();

            final String actualOutput = solution1249.minRemoveToMakeValid(input);

            System.out.println(expectedOutput.contains(actualOutput)
                    ? String.format("For %s, CORRECT!!", input)
                    : String.format("For input %s, expected %s, actual %s", input, expectedOutput, actualOutput)
            );
        }
    }

}

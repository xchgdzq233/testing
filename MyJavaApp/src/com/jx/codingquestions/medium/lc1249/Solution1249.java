package com.jx.codingquestions.medium.lc1249;

import java.util.HashSet;
import java.util.Set;

import com.jx.annotations.Topics;
import com.jx.data.Topic;

/**
 * https://leetcode.com/problems/minimum-remove-to-make-valid-parentheses
 *
 * Given a string of "(", ")" and lowercase English characters,
 *     remove the minimum number of parentheses,
 *     so that the parentheses are valid in the string.
 *
 * For examples:
 * - abc(d(e)f)gh) -> abc(d(e)f)gh / abc(d(e)fgh) / abc(d(ef)gh)
 * - a(bc)((d)ef -> a(bc)(d)ef
 * - a)b(c)d -> ab(c)d
 * - ))(( -> 
 */
@Topics(value = { Topic.Stack })
public class Solution1249 {

    public String minRemoveToMakeValid(String s) {

        final char[] input = s.toCharArray();
        final Set<Integer> toSkip = new HashSet<>();

        // start from front, check closing to Skip
        int opening = 0;
        for (int i = 0; i < input.length; i++) {
            if ('(' == input[i]) {
                opening++;
            } else if (')' == input[i]) {
                if (opening > 0) {
                    opening--;
                } else {
                    toSkip.add(i);
                }
            }
        }

        // start from back, check opening to skip
        int closing = 0;
        for (int i = input.length - 1; i >= 0; i--) {
            if (')' == input[i]) {
                closing++;
            } else if ('(' == input[i]) {
                if (closing > 0) {
                    closing--;
                } else {
                    toSkip.add(i);
                }
            }
        }

        final StringBuilder result = new StringBuilder();
        for (int i = 0; i < input.length; i++) {
            if (!toSkip.contains(i)) {
                result.append(input[i]);
            }
        }

        return result.toString();
    }
}

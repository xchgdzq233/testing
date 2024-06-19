package com.jx.codingquestions.medium.lc1209;

import java.util.AbstractMap;
import java.util.Map;
import java.util.Stack;

import com.jx.annotations.Topics;
import com.jx.codingquestions.easy.lc1047.Solution1047;
import com.jx.data.Topic;

/**
 * https://leetcode.com/problems/remove-all-adjacent-duplicates-in-string-ii/
 * Related question: {@link Solution1047}.
 */
@Topics(value = { Topic.Stack })
public class Solution1209 {

    public String removeDuplicates(String s, int k) {

        final char[] chars = s.toCharArray();

        final Stack<Map.Entry<Character, Integer>> stack = new Stack<>();

        for (final char c : chars) {
            if (stack.isEmpty() || c != stack.peek().getKey()) {
                stack.push(new AbstractMap.SimpleEntry<>(c, 1));
            } else {
                final Map.Entry<Character, Integer> last = stack.pop();
                stack.push(new AbstractMap.SimpleEntry<>(c, last.getValue() + 1));
            }

            if (stack.peek().getValue() == k) {
                stack.pop();
            }
        }

        final StringBuilder sb = new StringBuilder();
        while (!stack.isEmpty()) {
            final Map.Entry<Character, Integer> last = stack.pop();
            for (int i = 0; i < last.getValue(); i++) {
                sb.insert(0, last.getKey());
            }
        }

        return sb.toString();
    }
}

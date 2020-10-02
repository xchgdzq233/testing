package com.jx.leetcode.hard;

import static java.util.stream.Collectors.toSet;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;
import java.util.Set;

public class Lc212 {

    private char[][] board;
    private Set<String> words;
    //= Stream.of("oath", "pea", "eat", "rain").collect(toSet());

    private List<String> result;

    public List<String> findWords(char[][] board, String[] words) {

        this.board = board;
        this.words = Arrays.stream(words).collect(toSet());
        result = new ArrayList<>();

        for (int r = 0; r < board.length; r++) {
            for (int c = 0; c < board[0].length; c++) {
                String s = Character.toString(board[r][c]);
                if (this.words.contains(s)) {
                    result.add(s);
                }

                findWordsSub(new StringBuilder(s), r, c, r - 1, c);
                findWordsSub(new StringBuilder(s), r, c, r, c + 1);
                findWordsSub(new StringBuilder(s), r, c, r + 1, c);
                findWordsSub(new StringBuilder(s), r, c, r, c - 1);
            }
        }

        return result;
    }

    private void findWordsSub(StringBuilder prevString, int prevRow, int prevCol, int curRow, int curCol) {

        if(prevr)
    }
}

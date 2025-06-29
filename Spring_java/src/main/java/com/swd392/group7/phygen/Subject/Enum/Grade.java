package com.swd392.group7.phygen.Subject.Enum;

public enum Grade {
    TEN("10"),
    ELEVEN("11"),
    TWELVE("12");

    private final String value;

    Grade(String value) {
        this.value = value;
    }

    public String getValue() {
        return value;
    }

    public static Grade fromValue(String value) {
        for (Grade grade : Grade.values()) {
            if (grade.value.equals(value)) {
                return grade;
            }
        }
        throw new IllegalArgumentException("Unknown grade value: " + value);
    }
}

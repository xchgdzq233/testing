package com.jx.annotations;

import java.lang.annotation.ElementType;
import java.lang.annotation.Retention;
import java.lang.annotation.RetentionPolicy;
import java.lang.annotation.Target;

import com.jx.data.Topic;

@Target({ ElementType.TYPE })
@Retention(RetentionPolicy.CLASS)
public @interface Topics {

    Topic[] value() default {};
}

package org.nure.gotrip.controller;

import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;

import javax.servlet.http.Cookie;
import javax.servlet.http.HttpServletResponse;

@Controller
@RequestMapping("/test")
public class TestController {

    @GetMapping("/session")
    public void useSession(HttpServletResponse response){
        response.addCookie(new Cookie("lol", "kek"));
    }

}

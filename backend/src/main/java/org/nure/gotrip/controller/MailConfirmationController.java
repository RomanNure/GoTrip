package org.nure.gotrip.controller;

import org.nure.gotrip.exception.NotFoundUserException;
import org.nure.gotrip.model.RegisteredUser;
import org.nure.gotrip.service.RegisteredUserService;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;
import org.springframework.ui.ModelMap;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.servlet.ModelAndView;

@Controller
public class MailConfirmationController {

    private static final Logger logger = LoggerFactory.getLogger(MailConfirmationController.class);

    private RegisteredUserService userService;

    @Autowired
    public MailConfirmationController(RegisteredUserService userService) {
        this.userService = userService;
    }

    @GetMapping("/user/email/confirm")
    public ModelAndView confirmEmail(ModelMap model, @RequestParam long userId){
        model.addAttribute("attribute", "redirectWithRedirectPrefix");

        RegisteredUser user;
        try {
            user = userService.findById(userId);
        } catch (NotFoundUserException e) {
            logger.info("Cannot find user", e);
            return new ModelAndView("redirect:http://gotrips.dcxv.com:3000/fhskfha", model);
        }
        user.setEmailConfirmed(true);
        userService.update(user);

        return new ModelAndView("redirect:/OkPage.html", model);
    }
}

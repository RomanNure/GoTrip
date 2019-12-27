package org.nure.gotrip.controller.custom;

import org.modelmapper.ModelMapper;
import org.nure.gotrip.controller.response.ConflictException;
import org.nure.gotrip.controller.response.InternalServerException;
import org.nure.gotrip.controller.response.NotFoundException;
import org.nure.gotrip.dto.TourDto;
import org.nure.gotrip.exception.NotFoundUserException;
import org.nure.gotrip.exception.NotUniqueAdministratorException;
import org.nure.gotrip.exception.NotUniqueCompanyException;
import org.nure.gotrip.exception.NotUniqueTourException;
import org.nure.gotrip.model.*;
import org.nure.gotrip.service.AdministratorService;
import org.nure.gotrip.service.CompanyService;
import org.nure.gotrip.service.RegisteredUserService;
import org.nure.gotrip.service.TourService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.transaction.annotation.Transactional;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;

import java.util.Arrays;
import java.util.UUID;
import java.util.stream.Collectors;

@Controller
@RequestMapping("/custom/tour")
public class CustomTourController {

    private ModelMapper modelMapper;
    private TourService tourService;
    private RegisteredUserService registeredUserService;
    private AdministratorService administratorService;
    private CompanyService companyService;

    @Autowired
    public CustomTourController(ModelMapper modelMapper, TourService tourService, RegisteredUserService registeredUserService, AdministratorService administratorService, CompanyService companyService) {
        this.modelMapper = modelMapper;
        this.tourService = tourService;
        this.registeredUserService = registeredUserService;
        this.administratorService = administratorService;
        this.companyService = companyService;
    }

    @Transactional
    @PostMapping("/create")
    public ResponseEntity createTour(@RequestBody TourDto tourDto) {
        RegisteredUser user;
        try {
            user = registeredUserService.findById(tourDto.getIdAdministrator());
        } catch (NotFoundUserException e) {
            throw new NotFoundException(e.getMessage());
        }

        try {
            Company company = createCompany(user, tourDto.getName());
            Administrator administrator = createAdministrator(user, company);

            Tour tour = modelMapper.map(tourDto, Tour.class);
            boolean cond = tourDto.getPhotosUrl() != null;
            if(cond) {
                tour.setPhotos(Arrays.stream(tourDto.getPhotosUrl())
                        .map(stringUrl -> new TourPhoto(stringUrl, tour))
                        .collect(Collectors.toList()));
            }
            tour.setPricePerPerson(0.0);
            tour.setCustom(true);
            tour.setAdministrator(administrator);
            tour.setMaxParticipants(1);
            Tour newTour = tourService.add(tour);
            return new ResponseEntity<>(newTour, HttpStatus.OK);
        } catch (NotUniqueTourException e) {
            throw new ConflictException(e.getMessage());
        }catch(Exception e){
            throw new InternalServerException(e.getMessage());
        }
    }

    private Company createCompany(RegisteredUser user, String tourName) throws NotFoundUserException, NotUniqueCompanyException {
        Company company = new Company();
        company.setName(tourName+"_company_" + UUID.randomUUID().toString().substring(0, 3));
        company.setOwner(user);
        company.setEmail(user.getEmail());
        return companyService.add(company);
    }

    private Administrator createAdministrator(RegisteredUser user, Company company) throws NotUniqueAdministratorException {
        Administrator administrator = new Administrator();
        administrator.setRegisteredUser(user);
        administrator.setCompany(company);
        return administratorService.addAdministrator(administrator);
    }
}

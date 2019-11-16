package org.nure.gotrip.repository;

import org.nure.gotrip.dto.FilterUnit;
import org.nure.gotrip.exception.RuntimeDatabaseException;
import org.springframework.stereotype.Component;

import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.ArrayList;
import java.util.List;

@Component
public class TourJdbcRepository {

    private static final String DB_URL = "jdbc:postgresql://ec2-46-137-187-23.eu-west-1.compute.amazonaws.com:5432/df5a7i8te7u5ui";
    private static final String USER = "xmmbzrrpzmqumh";
    private static final String PASSWORD = "2278805c1d508f2c8ad3ff2b191ee6eb7282d259866de7502dc0ed945c04bf08";

    private static final String QUERY_START = "SELECT tours.tour_id " +
            "FROM (((tours left join administrators on tours.administrator_id = administrators.administrator_id)" +
            " left join tour_photos on tours.tour_id = tour_photos.tour_id)" +
            " left join participating on tours.tour_id = participating.tour_id)";

    public List<Long> getIdToursByCriteria(FilterUnit filterUnit) {
        String query = generateQuery(filterUnit);
        List<Long> result = new ArrayList<>();
        try(Connection connection = DriverManager.getConnection(DB_URL, USER, PASSWORD)){
            PreparedStatement statement = connection.prepareStatement(query);
            ResultSet resultSet = statement.executeQuery();
            while (resultSet.next()){
                result.add(resultSet.getLong(1));
            }
        }catch(SQLException e){
            throw new RuntimeDatabaseException(e);
        }
        return result;
    }


    private static String generateQuery(FilterUnit filterUnit){
        StringBuilder builder = new StringBuilder(QUERY_START);

        boolean whereSet = false;

        //masks
        if(filterUnit.getSearch().get("tourNameSubstr") != null){
            builder.append(" WHERE LOWER(tours.name) LIKE LOWER('%").append(filterUnit.getSearch().get("tourNameSubstr")).append("%')");
            whereSet = true;
        }

        if(filterUnit.getSearch().get("tourLocationSubstr") != null){
            if(!whereSet) {
                builder.append(" WHERE LOWER(tours.location) LIKE LOWER('%").append(filterUnit.getSearch().get("tourLocationSubstr")).append("%')");
                whereSet = true;
            }else{
                builder.append(" AND LOWER(tours.location) LIKE LOWER('%").append(filterUnit.getSearch().get("tourLocationSubstr")).append("%')");
            }
        }

        //filtering
        if(filterUnit.getFilter("priceFilter") != null){
            FilterUnit.Filter priceFilter = filterUnit.getFilter("priceFilter");
            if(!whereSet) {
                builder.append(" WHERE price_per_person BETWEEN ").append(priceFilter.getFrom()).append(" AND ").append(priceFilter.getTo());
                whereSet = true;
            }else{
                builder.append(" AND price_per_person BETWEEN ").append(priceFilter.getFrom()).append(" AND ").append(priceFilter.getTo());
            }
        }

        if(filterUnit.getFilter("startDateFilter") != null){
            FilterUnit.Filter durationFilter = filterUnit.getFilter("startDateFilter");
            if(whereSet){
                builder.append(" AND start_date_time BETWEEN '").append(durationFilter.getFrom()).append("' AND '").append(durationFilter.getTo()).append("'");
            }else{
                whereSet = true;
                builder.append(" WHERE start_date_time BETWEEN '").append(durationFilter.getFrom()).append("' AND '").append(durationFilter.getTo()).append("'");
            }
        }

        if(filterUnit.getFilter("participantsFilter") != null){
            FilterUnit.Filter durationFilter = filterUnit.getFilter("participantsFilter");
            if(whereSet){
                builder.append(" AND max_participants BETWEEN ").append(durationFilter.getFrom()).append(" AND ").append(durationFilter.getTo());
            }else{
                builder.append(" WHERE max_participants BETWEEN ").append(durationFilter.getFrom()).append(" AND ").append(durationFilter.getTo());
            }
        }

        //sorting
        if(filterUnit.getSortingCriterion().equals("price")){
            builder.append(" ORDER BY price_per_person ASC");
        }

        return builder.toString();
    }
}

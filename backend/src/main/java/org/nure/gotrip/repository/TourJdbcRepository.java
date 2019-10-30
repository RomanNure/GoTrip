package org.nure.gotrip.repository;

import org.nure.gotrip.dto.FilterUnit;
import org.nure.gotrip.exception.RuntimeDatabaseException;
import org.springframework.stereotype.Component;

import java.sql.*;
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

        List<FilterUnit.Filter> filters = filterUnit.getFilters();
        boolean whereSet = false;

        //masks
        if(filterUnit.getTourSubstring() != null){
            builder.append(" WHERE tours.name LIKE '%").append(filterUnit.getTourSubstring()).append("%'");
            whereSet = true;
        }

        if(filterUnit.getLocationSubstring() != null){
            if(!whereSet) {
                builder.append(" WHERE tours.name LIKE '%").append(filterUnit.getTourSubstring()).append("%'");
                whereSet = true;
            }else{
                builder.append(" AND tours.name LIKE '%").append(filterUnit.getTourSubstring()).append("%'");
            }
        }

        //filtering
        if(filters.contains(new FilterUnit.Filter("price"))){
            FilterUnit.Filter priceFilter = filterUnit.getFilterByName("price");
            if(!whereSet) {
                builder.append(" WHERE price_per_person BETWEEN ").append(priceFilter.getFrom()).append(" AND ").append(priceFilter.getTo());
                whereSet = true;
            }else{
                builder.append(" AND price_per_person BETWEEN ").append(priceFilter.getFrom()).append(" AND ").append(priceFilter.getTo());
            }
        }

        if(filters.contains(new FilterUnit.Filter("start"))){
            FilterUnit.Filter durationFilter = filterUnit.getFilterByName("start");
            if(whereSet){
                builder.append(" AND start_date_time BETWEEN '").append(durationFilter.getFrom()).append("' AND '").append(durationFilter.getTo()).append("'");
            }else{
                whereSet = true;
                builder.append(" WHERE start_date_time BETWEEN '").append(durationFilter.getFrom()).append("' AND '").append(durationFilter.getTo()).append("'");
            }
        }

        if(filters.contains(new FilterUnit.Filter("participants"))){
            FilterUnit.Filter durationFilter = filterUnit.getFilterByName("participants");
            if(whereSet){
                builder.append(" AND max_participants BETWEEN ").append(durationFilter.getFrom()).append(" AND ").append(durationFilter.getTo());
            }else{
                builder.append(" WHERE max_participants BETWEEN ").append(durationFilter.getFrom()).append(" AND ").append(durationFilter.getTo());
            }
        }

        //sorting
        List<String> sortings = filterUnit.getSortingCriteria();
        if(sortings.contains("price")){
            builder.append(" ORDER BY price_per_person ASC");
        }

        return builder.toString();
    }
}

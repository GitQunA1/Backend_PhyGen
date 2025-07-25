package com.swd392.group7.phygen.Dashboard;

import com.swd392.group7.phygen.Supabase.FileEntryDto;
import com.swd392.group7.phygen.Supabase.SupabaseStorageService;
import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;

import java.time.ZoneId;
import java.time.ZonedDateTime;
import java.util.List;
import java.util.Map;
import java.util.stream.Collectors;
import java.util.stream.IntStream;

@Service
@RequiredArgsConstructor
public class DashboardService {

    private final SupabaseStorageService supabaseStorageService;

    public Map<Integer,Long> totalGenerateExam(int targetYear){
        Map<Integer, Long> monthlyCounts = IntStream.rangeClosed(1, 12)
                .boxed()
                .collect(Collectors.toMap(
                        monthNum -> monthNum,
                        monthNum -> 0L
                ));
        List<FileEntryDto> fileEntryDtoList = supabaseStorageService.listFileKeysAndCreationDatesInBucket();
        for (FileEntryDto entry : fileEntryDtoList ){
            if(entry.getLastModified() != null){
                ZonedDateTime zdt = entry.getLastModified().atZone(ZoneId.systemDefault());
                if (zdt.getYear() == targetYear) {
                    int month = zdt.getMonthValue();
                    monthlyCounts.merge(month, 1L, Long::sum);
                }
            }
        }
        return monthlyCounts;
    }

}

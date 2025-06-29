package com.swd392.group7.phygen.Account.Repository;

import com.swd392.group7.phygen.Account.Entity.Account;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.Optional;

@Repository
public interface AccountRepo extends JpaRepository<Account,Long> {

    Optional<Account> findByName(String userName);
}

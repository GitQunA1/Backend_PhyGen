package com.swd392.group7.phygen.Authen.Service;

import com.swd392.group7.phygen.Account.Entity.Account;
import com.swd392.group7.phygen.Account.Exception.AccountException;
import com.swd392.group7.phygen.Account.Repository.AccountRepo;
import lombok.RequiredArgsConstructor;
import org.springframework.http.HttpStatus;
import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.security.core.userdetails.UserDetailsService;
import org.springframework.security.core.userdetails.UsernameNotFoundException;
import org.springframework.stereotype.Service;

@Service
@RequiredArgsConstructor
public class AuthenticationService implements UserDetailsService {

    private final AccountRepo accountRepo;

    public Account findAccountByName(String userName){
        return accountRepo.findByName(userName).orElseThrow(()-> new AccountException("Cant found account",HttpStatus.NOT_FOUND));
    }

    @Override
    public UserDetails loadUserByUsername(String username) throws UsernameNotFoundException {
        Account account = findAccountByName(username);
        if(account!=null){

        }
        return account;
    }
}

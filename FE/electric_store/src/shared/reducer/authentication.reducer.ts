import { createAsyncThunk, createSlice, isFulfilled, isPending, SerializedError } from "@reduxjs/toolkit"
import axios, { AxiosResponse } from "axios"
import { ILoginProps, ISignUpProps } from "../models/auth"
import { AppThunk, useAppDispatch } from "../../config/store"
import Cookies from "universal-cookie"
import { useNavigate } from "react-router-dom"
import { AUTH } from "../models/auth.api.ts/authapi"


export interface IAccountProps {
    id: number,
    name: string,
    email: string,
    phone: string,
    address: string,
    dateOfBirth: string,
    roleId: number,
    avatarUrl: string,
    gender: string,
    status: boolean
}

export const initialState = {
    loading: false,
    isAuthenticated: false,
    loginSuccess: false,
    loginError: false,
    errorMessage: null as unknown as string,
    account: null as unknown,
    message: ""
}

export type AuthenticationState = Readonly<typeof initialState>

export const getAccount = createAsyncThunk("authentication/get_account", async (jwt_token: string) => await axios.get<IAccountProps>(`https://localhost:7152/api/Auth/login/get-user-by-token/${jwt_token}`))

export const authenticate = createAsyncThunk("authentication/login", async (auth: ILoginProps) => await axios.post("https://localhost:7152/api/Auth/login", auth))

export const login: (username: string, password: string) => AppThunk = (email: string, password: string) => async dispatch => {
    const cookie = new Cookies()
    const result = await dispatch(authenticate({ email, password }));
    const response = result.payload as AxiosResponse
    const jwt_token = response.data.tokenString
    const expiration = response.data.expiration

    if (jwt_token && expiration) {
        cookie.set("jwt-token", jwt_token, {
            expires: new Date(expiration)
        })

        const account = await dispatch(getAccount(jwt_token));
        const response = account.payload as AxiosResponse
        cookie.set("account", response.data)
    }

}

export const signup = createAsyncThunk("authentication/signup", async (formSignUp: ISignUpProps) => {
    const requestUrl = axios.post(`${AUTH.USER.CREATEUSER}`, formSignUp)
    return requestUrl
})

export const logout = () => {
    const cookie = new Cookies()

    if (cookie.get("jwt-token") != null) {
        cookie.remove("jwt-token")
        cookie.remove("account")
        logoutSession()
    }

}


export const AuthenticationSlice = createSlice({
    name: "authentication",
    initialState: initialState as AuthenticationState,
    reducers: {
        logoutSession() {
            return {
                ...initialState
            }
        }
    },
    extraReducers(builder) {
        builder
            .addCase(authenticate.pending, (state, action) => ({
                ...initialState,
                loading: true
            }))
            .addCase(authenticate.rejected, (state, action) => ({
                ...initialState,
                loginError: true,

            }))
            .addCase(authenticate.fulfilled, (state, action) => (
                {
                    ...state,
                    loading: false,
                    loginError: false,
                    loginSuccess: true
                }
            ))

            .addCase(signup.fulfilled, (state, action) => {
                return {
                    ...state,
                    loading: false,
                    message:"Tạo tài khoản thành công"
                }
            })


            .addMatcher(isPending(getAccount, signup), (state, action) => {
                return {
                    ...state,
                    loading: true
                }
            })


            .addMatcher(isFulfilled(getAccount), (state, action) => {
                const { data } = action.payload
                return {
                    ...state,
                    loading: false,
                    account: data
                }
            })

    }
})

export const { logoutSession } = AuthenticationSlice.actions

export default AuthenticationSlice.reducer
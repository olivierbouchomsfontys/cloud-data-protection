import { TypedUseSelectorHook, useDispatch, useSelector } from 'react-redux'
import {AppDispatch, RootState} from "../stores/Store";

export const useAppDispatch = () => useDispatch<AppDispatch>();
export const useAppSelector: TypedUseSelectorHook<RootState> = useSelector;
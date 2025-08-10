import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { finalize } from 'rxjs';
import { LoaderService } from '../../services/loadingService/loader.service';

export const headerInterceptor: HttpInterceptorFn = (req, next) => {
  const accessToken = localStorage.getItem('accessToken');

  const loadingService = inject(LoaderService);
  if (!req.headers.has('X-No-Loader')) {
    loadingService.showLoading();
  }

  const clonedReq = req.clone({
    setHeaders: {
      Authorization: `Bearer ${accessToken}`,
    },
  });

  //   return next(clonedReq);
  return next(clonedReq).pipe(
    finalize(() => {
      if (!clonedReq.headers.has('X-No-Loader')) {
        loadingService.HideLoading();
      }
    })
  );
};

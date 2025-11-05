import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export function passwordMatchValidator(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const password = control.get('password');
    const confirmPassword = control.get('confirmPassword');
    if (!password || !confirmPassword) {
      return null;
    }
    if (password.value !== confirmPassword.value) {
      confirmPassword.setErrors({ passwordMissMatch: true });
      return { passwordMissMatch: true };
    } else {
      confirmPassword.setErrors(null);
      return null;
    }
  };
}

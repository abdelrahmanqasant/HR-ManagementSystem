import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export function minPeriodValidator(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const arrival = control.get('arrival')?.value;
    const departure = control.get('departure')?.value;

    if (arrival && departure) {
      const arrivalTime = new Date(arrival);
      const departureTime = new Date(departure);
      const durationMillis = departureTime.getTime() - arrivalTime.getTime();
      const minDurationMillis = 2 * 60 * 60 * 1000;

      if (durationMillis < minDurationMillis) {
        return { minPeriodViolation: true };
      }
    }

    return null;
  };
}

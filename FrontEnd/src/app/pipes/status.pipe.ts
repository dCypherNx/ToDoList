// src/app/pipes/status.pipe.ts
import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'statusPipe' })
export class StatusPipe implements PipeTransform {
  transform(value: number): string {
    switch (value) {
      case 1:
        return 'Concluída';
      case 0:
        return 'Pendente';
      case -1:
        return 'Cancelada';
      default:
        return 'Desconhecido';
    }
  }
}
